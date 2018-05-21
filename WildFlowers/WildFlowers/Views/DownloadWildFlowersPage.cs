using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PCLStorage;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Threading;

namespace PortableApp
{
    public partial class DownloadWildFlowersPage : ViewHelpers
    {
        public EventHandler InitFinishedDownload;
        public EventHandler InitCancelDownload;
        bool updatePlants;
        bool resyncPlants;
        bool clearDatabase;
        WildFlowerSetting datePlantDataUpdatedLocally;
        WildFlowerSetting datePlantDataUpdatedOnServer;
        List<WildFlowerSetting> imageFilesToDownload;
        ObservableCollection<WildFlower> plants;
        //ObservableCollection<WoodyGlossary> terms;
        ProgressBar progressBar = new ProgressBar();
        Label downloadLabel = new Label { Text = "", TextColor = Color.White, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center };
        Button cancelButton;
        CancellationTokenSource tokenSource;
        CancellationToken token;
        HttpClient client = new HttpClient();
        IFolder rootFolder = FileSystem.Current.LocalStorage;
        bool finished = false;
        protected async override void OnAppearing()
        {
            downloadLabel.Text = "Connecting ...";
            // Initialize CancellationToken
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            // Initialize progressbar and page
            progressBar.Progress = 0;
            base.OnAppearing();

            // Get all plants from external API call, store them in a collection
            plants = new ObservableCollection<WildFlower>(await externalConnection.GetAllPlants());
            //terms = new ObservableCollection<WoodyGlossary>(await externalConnection.GetAllTerms());

            // Save plants to the database
            if (updatePlants && !token.IsCancellationRequested)
                await StartDownload(token);

            if (token.IsCancellationRequested)
                CancelDownload();

            // Save images to the database
            //if (imageFilesToDownload.Count > 0 && downloadImages && !token.IsCancellationRequested)
            //    await UpdatePlantImages(token);

            FinishDownload();
        }

        public DownloadWildFlowersPage(bool updatePlantsNow, WildFlowerSetting dateLocalPlantDataUpdated, WildFlowerSetting datePlantDataUpdated, List<WildFlowerSetting> imageFilesNeedingDownloaded, bool downloadImagesFromServer, bool resyncplants, bool clearDatabase)
        {
            updatePlants = updatePlantsNow;
            this.resyncPlants = resyncplants;
            this.clearDatabase = clearDatabase;
            datePlantDataUpdatedLocally = dateLocalPlantDataUpdated;
            datePlantDataUpdatedLocally.valuetimestamp = DateTime.Now; 

            datePlantDataUpdatedOnServer = datePlantDataUpdated;
            imageFilesToDownload = (imageFilesNeedingDownloaded == null) ? new List<WildFlowerSetting>() : imageFilesNeedingDownloaded;
            //downloadImages = downloadImagesFromServer;

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid
            {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000")
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add label
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            innerContainer.Children.Add(downloadLabel, 0, 1);

            // Add progressbar
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(progressBar, 0, 2);

            // Add dismiss button
            cancelButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CANCEL",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            cancelButton.Clicked += CancelDownload;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(cancelButton, 0, 3);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private void FinishDownload()
        {
            InitFinishedDownload?.Invoke(this, EventArgs.Empty);
        }

        private void CancelDownload(object sender, EventArgs e)
        {
            progressBar.IsVisible = false;
            downloadLabel.Text = "Canceling Download, One Moment...";

            tokenSource.Cancel();
        }

        private void CancelDownload()
        {
            while (true)
            {
                try
                {
                    ClearRepositories();
                    ClearLocalRepositories();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ex {0}", e.Message);
                }
            }
            InitCancelDownload?.Invoke(this, EventArgs.Empty);
        }


        // Get plants from MobileApi server and save locally
        public async Task StartDownload(CancellationToken token)
        {
            try
            {
                if (resyncPlants)
                {
                    ClearLocalRepositories();
                }
                ClearRepositories();

                try
                {
                    IFolder folder = await rootFolder.GetFolderAsync("Images");
                    await folder.DeleteAsync();
                }
                catch (Exception e) { }


                await UpdatePlants(token);
                //App.WoodyPlantImageRepoLocal = new WoodyPlantImageRepositoryLocal(App.WoodyPlantImageRepo.GetAllWoodyPlantImages());
            }
            catch (OperationCanceledException e)
            {
                downloadLabel.Text = "Download Canceled!";
                Debug.WriteLine("Canceled UpdatePlants {0}", e.Message);
            }
            catch (Exception e)
            {
                downloadLabel.Text = "Error While Downloading Database!";
                Debug.WriteLine("ex {0}", e.Message);
            }
        }

        public async Task UpdatePlants(CancellationToken token)
        {


            if (imageFilesToDownload.Count > 0)
            {
                try
                {
                    long receivedBytes = 0;
                    long? totalBytes = imageFilesToDownload.Sum(x => x.valueint);

                    await progressBar.ProgressTo(0, 1, Easing.Linear);
                    downloadLabel.Text = "Beginning Download...";

                    //Downlod Plant Data
                    //await Task.Run(() => { UpdatePlantConcurrently(token); });
                    await UpdatePlantConcurrently(token);


                    // IFolder interface from PCLStorage; create or open imagesZipped folder (in Library/Images)    

                    IFolder folder = await rootFolder.CreateFolderAsync("Images", CreationCollisionOption.OpenIfExists);
                    string folderPath = rootFolder.Path;

                    // Get image file setting records from MobileApi to determine which files to download
                    // TODO: Limit this to only the files needed, not just every file
                    totalBytes = imageFilesToDownload.Sum(x => x.valueint);

                    // For each setting, get the corresponding zip file and save it locally
                    foreach (WildFlowerSetting imageFileToDownload in imageFilesToDownload)
                    {
                        if (token.IsCancellationRequested) { break; };
                        Stream webStream = await externalConnection.GetImageZipFiles(imageFileToDownload.valuetext.Replace(".zip", ""));
                        ZipInputStream zipInputStream = new ZipInputStream(webStream);
                        ZipEntry zipEntry = zipInputStream.GetNextEntry();
                        while (zipEntry != null)
                        {
                            if (token.IsCancellationRequested)
                            {
                                break;
                            };
                            //token.ThrowIfCancellationRequested();
                            String entryFileName = zipEntry.Name;
                            // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                            // Optionally match entrynames against a selection list here to skip as desired.

                            byte[] buffer = new byte[4096];

                            IFile file = await folder.CreateFileAsync(entryFileName, CreationCollisionOption.OpenIfExists);
                            using (Stream localStream = await file.OpenAsync(FileAccess.ReadAndWrite))
                            {
                                StreamUtils.Copy(zipInputStream, localStream, buffer);
                            }
                            receivedBytes += zipEntry.Size;
                            //Double percentage = (((double)plantsSaved * 100000) + (double)receivedBytes) / (((plants.Count + terms.Count) * 100000) + (double)totalBytes);
                            Double percentage = ((double)receivedBytes / (double)totalBytes);
                            await progressBar.ProgressTo(percentage, 1, Easing.Linear);
                            zipEntry = zipInputStream.GetNextEntry();

                            if (Math.Round(percentage * 100) < 100)
                            {
                                downloadLabel.Text = "Downloading Plant Data..." + Math.Round(percentage * 100) + "%";
                            }
                            else
                            {
                                downloadLabel.Text = "Finishing Download...";
                            }

                        }

                        if (!token.IsCancellationRequested)
                        {
                            //downloadImages = true;
                            await App.WildFlowerSettingsRepo.AddSettingAsync(new WildFlowerSetting { name = "ImagesZipFile", valuebool = true });
                            await App.WildFlowerSettingsRepo.AddSettingAsync(new WildFlowerSetting { name = "ImagesZipFile", valuetimestamp = imageFileToDownload.valuetimestamp, valuetext = imageFileToDownload.valuetext });



                            // App.WoodyPlantImageRepoLocal = new WoodyPlantImageRepositoryLocal(App.WoodyPlantImageRepo.GetAllWoodyPlantImages());
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine("Canceled Downloading of Images {0}", e.Message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ex {0}", e.Message);
                }
            }

        }
        public async Task UpdatePlantConcurrently(CancellationToken token)
        {
            try
            {
                if (!token.IsCancellationRequested)
                {
                    await App.WildFlowerRepo.AddOrUpdateAllPlantsAsync(plants);
                }
                if (!token.IsCancellationRequested)
                {
                    await App.WildFlowerSettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);
                }
                if (!token.IsCancellationRequested)
                {
                    datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
                    App.WildFlowerRepoLocal = new WildFlowerRepositoryLocal(App.WildFlowerRepo.GetAllWildFlowers());
                }

            }
            catch (Exception e) { Debug.WriteLine("ex {0}", e.Message); };

            finished = true;
        }

        private void ClearRepositories()
        {
            //Clear Repositories
            App.WildFlowerRepo.ClearWildFlowers();
            // App.WoodyPlantImageRepo.ClearWoodyImages();
            App.WildFlowerSettingsRepo.ClearWildFlowerSettings();
        }

        private void ClearLocalRepositories()
        {
            try { App.WildFlowerRepoLocal.ClearWildFlowersLocal(); } catch (Exception e) { }
            //try { App.WoodyPlantImageRepoLocal.ClearWoodyImagesLocal(); } catch (Exception e) { }
        }
    }
}
