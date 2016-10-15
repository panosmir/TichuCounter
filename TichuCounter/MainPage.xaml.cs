using System;
using TichuCounter.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TichuCounter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Score> Scores; //Observable Collection of "Score" model for Scores because we want live update when something is changed
        private int scoreA, scoreB; //Scores 
        private Boolean hasPoints; //Flag for knowing if user has entered points. 
        private int counterA, counterB; //Total score counters
        private int sa, sb; //Temporary scores for parsing total textbox
        

        public MainPage()
        {
            this.InitializeComponent();
            
            //Disable of unnecessary XAML elements
            teamATextBox.Visibility = Visibility.Collapsed;
            teamBTextBox.Visibility = Visibility.Collapsed;
            teamASaveButton.Visibility = Visibility.Collapsed;
            teamBSaveButton.Visibility = Visibility.Collapsed;


            ResultListA.Visibility = Visibility.Collapsed;
            ResultListB.Visibility = Visibility.Collapsed;
            tichuMade.Visibility = Visibility.Collapsed;
            tichuLost.Visibility = Visibility.Collapsed;
            GtichuMade.Visibility = Visibility.Collapsed;
            GtichuLost.Visibility = Visibility.Collapsed;
            tichuMade2.Visibility = Visibility.Collapsed;
            tichuLost2.Visibility = Visibility.Collapsed;
            GtichuMade2.Visibility = Visibility.Collapsed;
            GtichuLost2.Visibility = Visibility.Collapsed;
            saveButton.Visibility = Visibility.Collapsed;
            oneTwoA.Visibility = Visibility.Collapsed;
            oneTwoB.Visibility = Visibility.Collapsed;
            teamAPointsTextBox.Visibility = Visibility.Collapsed;
            teamBPointsTextBox.Visibility = Visibility.Collapsed;
            hintFlyout.Visibility = Visibility.Collapsed;
            hintFlyButtonText.Visibility = Visibility.Collapsed;

            //Create new Observable Collection. Type is Score (look in Model folder)
            Scores = new ObservableCollection<Score>();

            //Usually we use TichuCounter on our Mobile Phone so we want to know what time is it. Will we play too much? 
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }
            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                this.feedbackButton.Visibility = Visibility.Visible;
            }
            
        }

        //Save button is for saving the score of the current round
        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            //Now we check if user pressed save without entering any points
            if (string.IsNullOrEmpty(teamAPointsTextBox.Text) && string.IsNullOrEmpty(teamBPointsTextBox.Text))
            {
                //if user neither check the checkbox or enter any points it shows a message.
                if (oneTwoA.IsChecked == true && oneTwoB.IsChecked == true)
                {
                    hasPoints = false;
                    var noEntries = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                    await noEntries.ShowAsync();
                    oneTwoA.IsChecked = false;
                    oneTwoB.IsChecked = false;
                    tichuMade.IsChecked = false;
                    tichuLost.IsChecked = false;
                    GtichuMade.IsChecked = false;
                    GtichuLost.IsChecked = false;
                    tichuMade2.IsChecked = false;
                    tichuLost2.IsChecked = false;
                    GtichuMade2.IsChecked = false;
                    GtichuLost2.IsChecked = false;
                    oneTwoA.IsChecked = false;
                    oneTwoB.IsChecked = false;
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                }
                else if (oneTwoA.IsChecked == true && ((tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true || oneTwoB.IsChecked == true)
                    && (tichuMade.IsChecked == true || GtichuMade.IsChecked == true || oneTwoA.IsChecked == true)))
                {
                    var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                    await wrongChecks.ShowAsync();
                    hasPoints = false;
                    counterA = 0;
                    counterB = 0;
                    tichuMade.IsChecked = false;
                    tichuLost.IsChecked = false;
                    GtichuMade.IsChecked = false;
                    GtichuLost.IsChecked = false;
                    tichuMade2.IsChecked = false;
                    tichuLost2.IsChecked = false;
                    GtichuMade2.IsChecked = false;
                    GtichuLost2.IsChecked = false;
                    oneTwoA.IsChecked = false;
                    oneTwoB.IsChecked = false;
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                }
                else if (oneTwoB.IsChecked == true && (tichuMade.IsChecked == true || GtichuMade.IsChecked == true || oneTwoA.IsChecked == true)
                    && (tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true || oneTwoB.IsChecked == true))
                {
                    var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                    await wrongChecks.ShowAsync();
                    hasPoints = false;
                    counterA -= scoreA;
                    counterB -= scoreB;
                    tichuMade.IsChecked = false;
                    tichuLost.IsChecked = false;
                    GtichuMade.IsChecked = false;
                    GtichuLost.IsChecked = false;
                    tichuMade2.IsChecked = false;
                    tichuLost2.IsChecked = false;
                    GtichuMade2.IsChecked = false;
                    GtichuLost2.IsChecked = false;
                    oneTwoA.IsChecked = false;
                    oneTwoB.IsChecked = false;
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                }
                else
                {

                    if (oneTwoA.IsChecked == true && tichuLost2.IsChecked == true)
                    {
                        scoreA = 200;
                        scoreB = -100;
                    }
                    else if (oneTwoB.IsChecked == true && tichuLost.IsChecked == true)
                    {
                        scoreB = 200;
                        scoreA = -100;
                    }
                    else if (oneTwoA.IsChecked == true && GtichuLost2.IsChecked == true)
                    {
                        scoreA = 200;
                        scoreB = -200;
                    }
                    else if (oneTwoB.IsChecked == true && GtichuLost.IsChecked == true)
                    {
                        scoreB = 200;
                        scoreA = -200;
                    }
                    //Some statements, it's all about Tichu rules. Now it's about OneTwo where user is not entering any points just click the checkbox
                    if (oneTwoA.IsChecked == true && oneTwoB.IsChecked == false)
                    {
                        scoreB = 0;
                        if (tichuMade.IsChecked == true)
                        {
                            scoreA = 300;
                            if (tichuLost2.IsChecked == true)
                                scoreB -= 100;
                            else if (GtichuLost2.IsChecked == true)
                                scoreB -= 200;
                        }
                        else if (tichuLost.IsChecked == true)
                        {
                            scoreA = 100;
                        }
                        else if (GtichuMade.IsChecked == true)
                        {
                            scoreA = 400;
                            if (tichuLost2.IsChecked == true)
                                scoreB -= 100;
                            else if (GtichuLost2.IsChecked == true)
                                scoreB -= 200;
                        }
                        else if (GtichuLost.IsChecked == true)
                        {
                            scoreA = 0;
                        }
                        else if (tichuLost2.IsChecked == true)
                        {
                            scoreA = 200;
                            scoreB -= 100;
                        }
                        else if (GtichuLost2.IsChecked == true)
                        {
                            scoreA = 200;
                            scoreB -= 200;
                        }
                        else if (tichuMade.IsChecked == false && tichuLost.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                            scoreA = 200;
                        else
                            scoreA = 0;
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        ResultListA.ItemsSource = Scores;
                        ResultListB.ItemsSource = Scores;

                        totalA.Text = Convert.ToString(counterA);
                        totalB.Text = Convert.ToString(counterB);
                        hasPoints = true;

                    }
                    else if (oneTwoB.IsChecked == true && oneTwoA.IsChecked == false)
                    {
                        scoreA = 0;
                        if (tichuMade2.IsChecked == true)
                        {
                            scoreB = 300;
                            if (tichuLost.IsChecked == true)
                                scoreA -= 100;
                            else if (GtichuLost.IsChecked == true)
                                scoreA -= 200;
                        }
                        else if (tichuLost2.IsChecked == true)
                        {
                            scoreB = 100;
                        }
                        else if (GtichuMade2.IsChecked == true)
                        {
                            scoreB = 400;
                            if (tichuLost.IsChecked == true)
                                scoreA -= 100;
                            else if (GtichuLost.IsChecked == true)
                                scoreA -= 200;
                        }
                        else if (GtichuLost2.IsChecked == true)
                        {
                            scoreB = 0;
                        }
                        else if (tichuLost.IsChecked == true)
                        {
                            scoreB = 200;
                            scoreA -= 100;
                        }
                        else if (GtichuLost.IsChecked == true)
                        {
                            scoreB = 200;
                            scoreA -= 200;
                        }
                        else if (tichuMade2.IsChecked == false && tichuLost2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                            scoreB = 200;
                        else
                            scoreB = 0;

                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        ResultListA.ItemsSource = Scores;
                        ResultListB.ItemsSource = Scores;
                        totalA.Text = Convert.ToString(counterA);
                        totalB.Text = Convert.ToString(counterB);
                        hasPoints = true;
                    }
                }
            }
            //now we check if user entered points only in Team A. Some calculations below
            else if (string.IsNullOrEmpty(teamBPointsTextBox.Text))
            {
                if (Convert.ToInt32(teamAPointsTextBox.Text) % 5 != 0)
                {
                    var wrongScore = new Windows.UI.Popups.MessageDialog("Tichu points are \n-25, -20, -15, -10, -5,\n0, 5, 10, 15, 20 etc");
                    await wrongScore.ShowAsync();
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                    hasPoints = false;
                }
                else if (oneTwoA.IsChecked == true || oneTwoB.IsChecked == true)
                {
                    var wrongScore = new Windows.UI.Popups.MessageDialog("You can't check 1-2 if you have entered points.");
                    await wrongScore.ShowAsync();
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                    oneTwoA.IsChecked = false;
                    oneTwoB.IsChecked = false;
                    tichuMade.IsChecked = false;
                    tichuLost.IsChecked = false;
                    GtichuLost.IsChecked = false;
                    GtichuMade.IsChecked = false;
                    hasPoints = false;
                }
                else
                {
                    scoreA = Convert.ToInt32(teamAPointsTextBox.Text);
                    scoreA = Int32.Parse(teamAPointsTextBox.Text);

                    if ((scoreA <= 100 && scoreA >= 0) && scoreA % 5 == 0)
                    {
                        scoreB = 100 - scoreA;
                        if (tichuMade.IsChecked == true && tichuLost.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA += 100;
                        }
                        else if (tichuLost.IsChecked == true && tichuMade.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuMade.IsChecked == true && GtichuLost.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA += 200;
                        }
                        else if (GtichuLost.IsChecked == true && GtichuMade.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA -= 200;
                        }
                        if (tichuMade.IsChecked == true && tichuLost2.IsChecked == true)
                        {
                            scoreB -= 100;
                        }
                        else if (tichuMade.IsChecked == true && GtichuLost2.IsChecked == true)
                        {
                            scoreB -= 200;
                        }
                        else if (GtichuMade.IsChecked == true && tichuLost2.IsChecked == true)
                        {
                            scoreB -= 100;
                        }
                        else if (GtichuMade.IsChecked == true && GtichuLost2.IsChecked == true)
                        {
                            scoreB -= 200;
                        }
                        else if (tichuMade2.IsChecked == true)
                        {
                            scoreB += 100;
                        }
                        else if (tichuLost2.IsChecked == true)
                        {
                            scoreB -= 100;
                        }
                        else if (GtichuMade2.IsChecked == true)
                        {
                            scoreB += 200;
                        }
                        else if (GtichuLost2.IsChecked == true)
                        {
                            scoreB -= 200;
                        }
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if ((tichuMade.IsChecked == true || GtichuMade.IsChecked == true) && (tichuLost.IsChecked == true || GtichuLost.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                        else if ((tichuMade.IsChecked == true && tichuMade2.IsChecked == true) || (GtichuMade.IsChecked == true && tichuMade2.IsChecked == true) ||
                            (tichuMade.IsChecked == true && GtichuMade.IsChecked == true) || (tichuMade.IsChecked == true && GtichuMade2.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                    }
                    else if (scoreA == 200)
                    {
                        scoreA = 200;
                        scoreB = 0;
                        if (tichuMade.IsChecked == true && tichuLost.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA += 100;
                        }
                        else if (tichuLost.IsChecked == true && tichuMade.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuMade.IsChecked == true && GtichuLost.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA += 200;
                        }
                        else if (GtichuLost.IsChecked == true && GtichuMade.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA -= 200;
                        }
                        if (tichuLost2.IsChecked == true)
                            scoreB = -100;
                        else if (GtichuLost2.IsChecked == true)
                            scoreB = -200;
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if (tichuMade.IsChecked == true && GtichuMade.IsChecked == true)
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                        else if ((tichuLost.IsChecked == true || GtichuLost.IsChecked == true) || (tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                    }
                    else if (scoreA == 300)
                    {
                        scoreA = 300;
                        scoreB = 0;

                        if (tichuLost2.IsChecked == true)
                            scoreB = -100;
                        else if (GtichuLost2.IsChecked == true)
                            scoreB = -200;

                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if (tichuMade.IsChecked == true || tichuLost.IsChecked == true || GtichuMade.IsChecked == true || GtichuLost.IsChecked == true ||
                            tichuMade2.IsChecked == true ||  GtichuMade2.IsChecked == true)
                        {
                            var illigalChecks = new Windows.UI.Popups.MessageDialog("You can't check any of those if you have 300 points.");
                            await illigalChecks.ShowAsync();
                            hasPoints = false;
                            scoreA = 0;
                            scoreB = 0;
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                        }
                    }
                    else if (scoreA == 400)
                    {
                        scoreA = 400;
                        scoreB = 0;

                        if (tichuLost2.IsChecked == true)
                            scoreB = -100;
                        else if (GtichuLost2.IsChecked == true)
                            scoreB = -200;

                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if (tichuMade.IsChecked == true || tichuLost.IsChecked == true || GtichuMade.IsChecked == true || GtichuLost.IsChecked == true ||
                            tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true)
                        {
                            var illigalChecks = new Windows.UI.Popups.MessageDialog("You can't check any of those if you have 400 points.");
                            await illigalChecks.ShowAsync();
                            hasPoints = false;
                            scoreA = 0;
                            scoreB = 0;
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                        }
                    }
                    else if (scoreA < 0 && scoreA >= -25 && scoreA % 5 == 0)
                    {
                        scoreB = (2 * (scoreA * -1)) + (100 + scoreA);
                        if (tichuMade.IsChecked == true && tichuLost.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA += 100;
                        }
                        else if (tichuLost.IsChecked == true && tichuMade.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuMade.IsChecked == true && GtichuLost.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA += 200;
                        }
                        else if (GtichuLost.IsChecked == true && GtichuMade.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA -= 200;
                        }
                        if (tichuMade2.IsChecked == true)
                            scoreB += 100;
                        else if (tichuLost2.IsChecked == true)
                            scoreB -= 100;
                        else if (GtichuMade2.IsChecked == true)
                            scoreB += 200;
                        else if (GtichuLost2.IsChecked == true)
                            scoreB -= 200;
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                    }
                    else if ((scoreA <= 125 && scoreA >= 100) && scoreA % 5 == 0)
                    {
                        scoreB = scoreA - 100;
                        scoreB -= 2 * scoreB;

                        if (tichuMade.IsChecked == true && tichuLost.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA += 100;
                        }
                        else if (tichuLost.IsChecked == true && tichuMade.IsChecked == false && GtichuMade.IsChecked == false && GtichuLost.IsChecked == false)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuMade.IsChecked == true && GtichuLost.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA += 200;
                        }
                        else if (GtichuLost.IsChecked == true && GtichuMade.IsChecked == false && tichuMade.IsChecked == false && tichuLost.IsChecked == false)
                        {
                            scoreA -= 200;
                        }
                        if (tichuMade2.IsChecked == true)
                            scoreB += 100;
                        else if (tichuLost2.IsChecked == true)
                            scoreB -= 100;
                        else if (GtichuMade2.IsChecked == true)
                            scoreB += 200;
                        else if (GtichuLost2.IsChecked == true)
                            scoreB -= 200;
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                    }
                }
            }
            //now we check if user entered points only in Team B. Some calculations below
            else if (string.IsNullOrEmpty(teamAPointsTextBox.Text))
            {
                if (Convert.ToInt32(teamBPointsTextBox.Text) % 5 != 0)
                {
                    var wrongScore = new Windows.UI.Popups.MessageDialog("Tichu points are \n-25, -20, -15, -10, -5,\n0, 5, 10, 15, 20 etc");
                    await wrongScore.ShowAsync();
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                    hasPoints = false;
                }
                else if (oneTwoA.IsChecked == true || oneTwoB.IsChecked == true)
                {
                    var wrongScore = new Windows.UI.Popups.MessageDialog("You can't check 1-2 if you have entered points.");
                    await wrongScore.ShowAsync();
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                    oneTwoA.IsChecked = false;
                    oneTwoB.IsChecked = false;
                    tichuMade2.IsChecked = false;
                    tichuLost2.IsChecked = false;
                    GtichuMade2.IsChecked = false;
                    GtichuLost2.IsChecked = false;
                    hasPoints = false;
                }
                else
                {
                    scoreB = Convert.ToInt32(teamBPointsTextBox.Text);
                    scoreB = Int32.Parse(teamBPointsTextBox.Text);

                    if ((scoreB <= 100 && scoreB >= 0) && scoreA % 5 == 0)
                    {
                        scoreA = 100 - scoreB;
                        if (tichuMade2.IsChecked == true && tichuLost2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB += 100;
                        }
                        else if (tichuLost2.IsChecked == true && tichuMade2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB -= 100;
                        }
                        else if (GtichuMade2.IsChecked == true && GtichuLost2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB += 200;
                        }
                        else if (GtichuLost2.IsChecked == true && GtichuMade2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB -= 200;
                        }
                        if (tichuMade2.IsChecked == true && tichuLost.IsChecked == true)
                        {
                            scoreA -= 100;
                        }
                        else if (tichuMade2.IsChecked == true && GtichuLost.IsChecked == true)
                        {
                            scoreA -= 200;
                        }
                        else if (GtichuMade2.IsChecked == true && tichuLost.IsChecked == true)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuMade2.IsChecked == true && GtichuLost.IsChecked == true)
                        {
                            scoreA -= 200;
                        }
                        else if (tichuMade.IsChecked == true)
                        {
                            scoreA += 100;
                        }
                        else if (tichuLost.IsChecked == true)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuMade.IsChecked == true)
                        {
                            scoreA += 200;
                        }
                        else if (GtichuLost.IsChecked == true)
                        {
                            scoreA -= 200;
                        }
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if ((tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true) && (tichuLost2.IsChecked == true || GtichuLost2.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                        else if ((tichuMade.IsChecked == true && tichuMade2.IsChecked == true) || (GtichuMade.IsChecked == true && tichuMade2.IsChecked == true) ||
                            (tichuMade.IsChecked == true && GtichuMade.IsChecked == true) || (tichuMade.IsChecked == true && GtichuMade2.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                    }
                    else if (scoreB == 200)
                    {
                        scoreB = 200;
                        scoreA = 0;
                        if (tichuMade2.IsChecked == true && tichuLost2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB += 100;
                        }
                        else if (tichuLost2.IsChecked == true && tichuMade2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB -= 100;
                        }
                        else if (GtichuMade2.IsChecked == true && GtichuLost2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB += 200;
                        }
                        else if (GtichuLost2.IsChecked == true && GtichuMade2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB -= 200;
                        }
                        if (tichuLost.IsChecked == true)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuLost.IsChecked == true)
                        {
                            scoreA -= 200;
                        }
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if (tichuMade2.IsChecked == true && GtichuMade2.IsChecked == true)
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                        else if ((tichuLost2.IsChecked == true || GtichuLost2.IsChecked == true) || (tichuMade.IsChecked == true || GtichuMade.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                            counterA -= scoreA;
                            counterB -= scoreB;
                            hasPoints = false;
                        }
                    }
                    else if (scoreB == 300)
                    {
                        scoreB = 300;
                        scoreA = 0;

                        if (tichuLost.IsChecked == true)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuLost.IsChecked == true)
                        {
                            scoreA -= 200;
                        }

                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if (tichuMade2.IsChecked == true || tichuLost2.IsChecked == true || GtichuMade2.IsChecked == true || GtichuLost2.IsChecked == true ||
                            tichuMade.IsChecked == true || GtichuMade.IsChecked == true)
                        {
                            var illigalChecks = new Windows.UI.Popups.MessageDialog("You can't check any of those if you have 300 points.");
                            await illigalChecks.ShowAsync();
                            hasPoints = false;
                            scoreA = 0;
                            scoreB = 0;
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                        }
                    }
                    else if (scoreB == 400)
                    {
                        scoreB = 400;
                        scoreA = 0;

                        if (tichuLost.IsChecked == true)
                        {
                            scoreA -= 100;
                        }
                        else if (GtichuLost.IsChecked == true)
                        {
                            scoreA -= 200;
                        }
                        counterA += scoreA;
                        counterB += scoreB;
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        hasPoints = true;
                        if (tichuMade2.IsChecked == true || tichuLost2.IsChecked == true || GtichuMade2.IsChecked == true || GtichuLost2.IsChecked == true ||
                            tichuMade.IsChecked == true || tichuLost.IsChecked == true || GtichuMade.IsChecked == true || GtichuLost.IsChecked == true)
                        {
                            var illigalChecks = new Windows.UI.Popups.MessageDialog("You can't check any of those if you have 400 points.");
                            await illigalChecks.ShowAsync();
                            hasPoints = false;
                            scoreA = 0;
                            scoreB = 0;
                            teamAPointsTextBox.Text = "";
                            teamBPointsTextBox.Text = "";
                            tichuMade.IsChecked = false;
                            tichuLost.IsChecked = false;
                            GtichuMade.IsChecked = false;
                            GtichuLost.IsChecked = false;
                            tichuMade2.IsChecked = false;
                            tichuLost2.IsChecked = false;
                            GtichuMade2.IsChecked = false;
                            GtichuLost2.IsChecked = false;
                        }
                    }
                    else if (scoreB < 0 && scoreB >= -25 && scoreA % 5 == 0)
                    {
                        scoreA = (2 * (scoreB * -1)) + (100 + scoreB);

                        if (tichuMade2.IsChecked == true && tichuLost2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB += 100;
                        }
                        else if (tichuLost2.IsChecked == true && tichuMade2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB -= 100;
                        }
                        else if (GtichuMade2.IsChecked == true && GtichuLost2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB += 200;
                        }
                        else if (GtichuLost2.IsChecked == true && GtichuMade2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB -= 200;
                        }
                        if (tichuMade.IsChecked == true)
                            scoreA += 100;
                        else if (tichuLost.IsChecked == true)
                            scoreA -= 100;
                        else if (GtichuMade.IsChecked == true)
                            scoreA += 200;
                        else if (GtichuLost.IsChecked == true)
                            scoreA -= 200;
                        counterA += scoreA;
                        counterB += scoreB;
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        hasPoints = true;
                    }
                    else if ((scoreB <= 125 && scoreB >= 100) && scoreA % 5 == 0)
                    {
                        scoreA = scoreB - 100;
                        scoreA -= 2 * scoreA;
                        if (tichuMade2.IsChecked == true && tichuLost2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB += 100;
                        }
                        else if (tichuLost2.IsChecked == true && tichuMade2.IsChecked == false && GtichuMade2.IsChecked == false && GtichuLost2.IsChecked == false)
                        {
                            scoreB -= 100;
                        }
                        else if (GtichuMade2.IsChecked == true && GtichuLost2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB += 200;
                        }
                        else if (GtichuLost2.IsChecked == true && GtichuMade2.IsChecked == false && tichuMade2.IsChecked == false && tichuLost2.IsChecked == false)
                        {
                            scoreB -= 200;
                        }
                        if (tichuMade.IsChecked == true)
                            scoreA += 100;
                        else if (tichuLost.IsChecked == true)
                            scoreA -= 100;
                        else if (GtichuMade.IsChecked == true)
                            scoreA += 200;
                        else if (GtichuLost.IsChecked == true)
                            scoreA -= 200;
                        counterA += scoreA;
                        counterB += scoreB;
                        teamBPointsTextBox.Text = Convert.ToString(scoreB);
                        teamAPointsTextBox.Text = Convert.ToString(scoreA);
                        hasPoints = true;
                    }
                }
            }
            //This is the case where the user enters manually the points of each team and doesn't use the app basic functionality
            else if (string.IsNullOrEmpty(teamAPointsTextBox.Text) == false && string.IsNullOrEmpty(teamBPointsTextBox.Text) == false)
            {
                //var scoreError = new Windows.UI.Popups.MessageDialog("You don't have to enter the points manually.\nTry to enter the score of the one team and see the magic!");
                //await scoreError.ShowAsync();
                scoreA = Convert.ToInt32(teamAPointsTextBox.Text);
                scoreA = Int32.Parse(teamAPointsTextBox.Text);
                scoreB = Convert.ToInt32(teamBPointsTextBox.Text);
                scoreB = Int32.Parse(teamBPointsTextBox.Text);
                if (((scoreA >= 0 && scoreA <= 100) && (scoreB >= 0 && scoreB <= 100)) && scoreA % 5 == 0 && scoreB % 5 == 0)
                {
                    if (scoreA + scoreB == 100)
                    {
                        if (tichuMade.IsChecked == true && (tichuLost.IsChecked == true || GtichuMade.IsChecked == true || GtichuLost.IsChecked == true
                            || oneTwoA.IsChecked == true || oneTwoB.IsChecked == true || tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true || oneTwoB.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else if (tichuMade2.IsChecked == true && (tichuLost2.IsChecked == true || GtichuMade2.IsChecked == true || GtichuLost2.IsChecked == true
                            || oneTwoA.IsChecked == true || oneTwoB.IsChecked == true || tichuMade.IsChecked == true || GtichuMade.IsChecked == true || oneTwoA.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else if (oneTwoA.IsChecked == true || oneTwoB.IsChecked == true)
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else
                        {
                            if (tichuMade.IsChecked == true)
                                scoreA += 100;
                            else if (tichuLost.IsChecked == true)
                                scoreA -= 100;
                            else if (GtichuMade.IsChecked == true)
                                scoreA += 200;
                            else if (GtichuLost.IsChecked == true)
                                scoreA -= 200;
                            if (tichuMade2.IsChecked == true)
                                scoreB += 100;
                            else if (tichuLost2.IsChecked == true)
                                scoreB -= 100;
                            else if (GtichuMade2.IsChecked == true)
                                scoreB += 200;
                            else if (GtichuLost2.IsChecked == true)
                                scoreB -= 200;
                            counterA += scoreA;
                            counterB += scoreB;
                            teamBPointsTextBox.Text = Convert.ToString(scoreB);
                            teamAPointsTextBox.Text = Convert.ToString(scoreA);
                            hasPoints = true;
                        }
                    }
                    else
                    {
                        var wrong = new Windows.UI.Popups.MessageDialog("You have entered incorrect points.");
                        await wrong.ShowAsync();
                        teamAPointsTextBox.Text = "";
                        teamBPointsTextBox.Text = "";
                        hasPoints = false;
                    }
                }
                else if ((scoreA < 0 && scoreA >= -25) && (scoreB > 100 && scoreB <= 125) && scoreA % 5 == 0 && scoreB % 5 == 0)
                {
                    if (scoreA + scoreB == 100)
                    {
                        if (tichuMade.IsChecked == true && (tichuLost.IsChecked == true || GtichuMade.IsChecked == true || GtichuLost.IsChecked == true
                            || oneTwoA.IsChecked == true || oneTwoB.IsChecked == true || tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true || oneTwoB.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else if (tichuMade2.IsChecked == true && (tichuLost2.IsChecked == true || GtichuMade2.IsChecked == true || GtichuLost2.IsChecked == true
                            || oneTwoA.IsChecked == true || oneTwoB.IsChecked == true || tichuMade.IsChecked == true || GtichuMade.IsChecked == true || oneTwoA.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else if (oneTwoA.IsChecked == true || oneTwoB.IsChecked == true)
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else
                        {
                            if (tichuMade.IsChecked == true)
                                scoreA += 100;
                            else if (tichuLost.IsChecked == true)
                                scoreA -= 100;
                            else if (GtichuMade.IsChecked == true)
                                scoreA += 200;
                            else if (GtichuLost.IsChecked == true)
                                scoreA -= 200;
                            if (tichuMade2.IsChecked == true)
                                scoreB += 100;
                            else if (tichuLost2.IsChecked == true)
                                scoreB -= 100;
                            else if (GtichuMade2.IsChecked == true)
                                scoreB += 200;
                            else if (GtichuLost2.IsChecked == true)
                                scoreB -= 200;
                            counterA += scoreA;
                            counterB += scoreB;
                            teamBPointsTextBox.Text = Convert.ToString(scoreB);
                            teamAPointsTextBox.Text = Convert.ToString(scoreA);
                            hasPoints = true;
                        }
                    }
                    else
                    {
                        var wrong = new Windows.UI.Popups.MessageDialog("You have entered incorrect points.");
                        await wrong.ShowAsync();
                        teamAPointsTextBox.Text = "";
                        teamBPointsTextBox.Text = "";
                        hasPoints = false;
                    }
                }
                else if ((scoreA >100 && scoreA <= 125) && (scoreB < 0 && scoreB >= -25) && scoreA % 5 == 0 && scoreB % 5 == 0)
                {
                    if (scoreA + scoreB == 100)
                    {
                        if (tichuMade.IsChecked == true && (tichuLost.IsChecked == true || GtichuMade.IsChecked == true || GtichuLost.IsChecked == true
                            || oneTwoA.IsChecked == true || oneTwoB.IsChecked == true || tichuMade2.IsChecked == true || GtichuMade2.IsChecked == true || oneTwoB.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else if (tichuMade2.IsChecked == true && (tichuLost2.IsChecked == true || GtichuMade2.IsChecked == true || GtichuLost2.IsChecked == true
                            || oneTwoA.IsChecked == true || oneTwoB.IsChecked == true || tichuMade.IsChecked == true || GtichuMade.IsChecked == true || oneTwoA.IsChecked == true))
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else if (oneTwoA.IsChecked == true || oneTwoB.IsChecked == true)
                        {
                            var wrongChecks = new Windows.UI.Popups.MessageDialog("Something is wrong with your checks!");
                            await wrongChecks.ShowAsync();
                            hasPoints = false;
                        }
                        else
                        {
                            if (tichuMade.IsChecked == true)
                                scoreA += 100;
                            else if (tichuLost.IsChecked == true)
                                scoreA -= 100;
                            else if (GtichuMade.IsChecked == true)
                                scoreA += 200;
                            else if (GtichuLost.IsChecked == true)
                                scoreA -= 200;
                            if (tichuMade2.IsChecked == true)
                                scoreB += 100;
                            else if (tichuLost2.IsChecked == true)
                                scoreB -= 100;
                            else if (GtichuMade2.IsChecked == true)
                                scoreB += 200;
                            else if (GtichuLost2.IsChecked == true)
                                scoreB -= 200;
                            counterA += scoreA;
                            counterB += scoreB;
                            teamBPointsTextBox.Text = Convert.ToString(scoreB);
                            teamAPointsTextBox.Text = Convert.ToString(scoreA);
                            hasPoints = true;
                        }
                    }
                    else
                    {
                        var wrong = new Windows.UI.Popups.MessageDialog("You have entered incorrect points.");
                        await wrong.ShowAsync();
                        teamAPointsTextBox.Text = "";
                        teamBPointsTextBox.Text = "";
                        hasPoints = false;
                    }
                }
                else
                {
                    var wrong = new Windows.UI.Popups.MessageDialog("You have entered incorrect points.");
                    await wrong.ShowAsync();
                    teamAPointsTextBox.Text = "";
                    teamBPointsTextBox.Text = "";
                    hasPoints = false;
                }
            }
            //now we use the flag to note the points of every round
            if (hasPoints == true && scoreA % 5 == 0 && scoreB % 5 == 0)
            {
                if (((scoreA < -225 || scoreA > 400) || (scoreB < -225 || scoreB > 400)) && (oneTwoA.IsChecked == false && oneTwoB.IsChecked == false))
                {
                    var wrongScore = new Windows.UI.Popups.MessageDialog("You have entered wrong score");
                    await wrongScore.ShowAsync();
                }
                else
                {
                    Scores.Add(new Score { resultA = teamAPointsTextBox.Text, resultB = teamBPointsTextBox.Text });

                    ResultListA.ItemsSource = Scores;
                    ResultListB.ItemsSource = Scores;

                    if (Scores.Count > 0)
                    {
                        hintButton.Visibility = Visibility.Collapsed;
                        hintTextBlock.Visibility = Visibility.Collapsed;
                        hintFlyout.Visibility = Visibility.Visible;
                        hintFlyButtonText.Visibility = Visibility.Visible;
                    }

                    totalA.Text = Convert.ToString(counterA);
                    totalB.Text = Convert.ToString(counterB);

                    //let the user know that the game has finished
                    if (finalScoreA.IsChecked == true)
                    {
                        if ((Convert.ToInt32(totalA.Text) >= 1000))
                        {
                            var winMessage = new Windows.UI.Popups.MessageDialog(teamATextBlock.Text + " wins the game with " + Convert.ToInt32(totalA.Text) + " points!!!");
                            winMessage.Commands.Add(new UICommand("New Game", null));
                            var command = await winMessage.ShowAsync();

                            if (command.Label == "New Game")
                            {
                                newGameButton_Click(sender, e);
                            }
                        }
                        else if (Convert.ToInt32(totalB.Text) >= 1000)
                        {
                            var winMessage = new Windows.UI.Popups.MessageDialog(teamBTextBlock.Text + " wins the game with " + Convert.ToInt32(totalB.Text) + " points");
                            winMessage.Commands.Add(new UICommand("New Game", null));
                            var command = await winMessage.ShowAsync();

                            if (command.Label == "New Game")
                            {
                                newGameButton_Click(sender, e);
                            }
                        }
                    }
                    else if (finalScoreB.IsChecked == true)
                    {
                        if (Convert.ToInt32(totalA.Text) >= 500)
                        {
                            var winMessage = new Windows.UI.Popups.MessageDialog(teamATextBlock.Text + " wins the game with " + Convert.ToInt32(totalA.Text) + " points!!!");
                            winMessage.Commands.Add(new UICommand("New Game", null));
                            winMessage.Commands.Add(new UICommand("One more? :)", null));
                            var command = await winMessage.ShowAsync();

                            if (command.Label == "New Game")
                            {
                                newGameButton_Click(sender, e);
                            }
                            else if (command.Label == "One more? :)")
                            {
                                if ((Convert.ToInt32(totalA.Text) >= 1000))
                                {
                                    winMessage = new Windows.UI.Popups.MessageDialog(teamATextBlock.Text + " wins the game with " + Convert.ToInt32(totalA.Text) + " points!!!");
                                    winMessage.Commands.Add(new UICommand("New Game", null));
                                    command = await winMessage.ShowAsync();

                                    if (command.Label == "New Game")
                                    {
                                        newGameButton_Click(sender, e);
                                    }
                                }
                                else if (Convert.ToInt32(totalB.Text) >= 1000)
                                {
                                    winMessage = new Windows.UI.Popups.MessageDialog(teamBTextBlock.Text + " wins the game with " + Convert.ToInt32(totalB.Text) + " points");
                                    winMessage.Commands.Add(new UICommand("New Game", null));
                                    command = await winMessage.ShowAsync();

                                    if (command.Label == "New Game")
                                    {
                                        newGameButton_Click(sender, e);
                                    }
                                }
                            }
                        }
                        else if (Convert.ToInt32(totalB.Text) >= 500)
                        {
                            var winMessage = new Windows.UI.Popups.MessageDialog(teamBTextBlock.Text + " wins the game with " + Convert.ToInt32(totalB.Text) + " points");
                            winMessage.Commands.Add(new UICommand("New Game", null));
                            winMessage.Commands.Add(new UICommand("One more? :)", null));

                            var command = await winMessage.ShowAsync();

                            if (command.Label == "New Game")
                            {
                                newGameButton_Click(sender, e);
                            }
                            else if (command.Label == "One more? :)")
                            {
                                if ((Convert.ToInt32(totalA.Text) >= 1000))
                                {
                                    winMessage = new Windows.UI.Popups.MessageDialog(teamATextBlock.Text + " wins the game with " + Convert.ToInt32(totalA.Text) + " points!!!");
                                    winMessage.Commands.Add(new UICommand("New Game", null));
                                    command = await winMessage.ShowAsync();

                                    if (command.Label == "New Game")
                                    {
                                        newGameButton_Click(sender, e);
                                    }
                                }
                                else if (Convert.ToInt32(totalB.Text) >= 1000)
                                {
                                    winMessage = new Windows.UI.Popups.MessageDialog(teamBTextBlock.Text + " wins the game with " + Convert.ToInt32(totalB.Text) + " points");
                                    winMessage.Commands.Add(new UICommand("New Game", null));
                                    command = await winMessage.ShowAsync();

                                    if (command.Label == "New Game")
                                    {
                                        newGameButton_Click(sender, e);
                                    }
                                }
                            }
                        }
                    }
                }

                //when the game is over we clear every elements to its default 
                teamAPointsTextBox.Text = "";
                teamBPointsTextBox.Text = "";
                tichuMade.IsChecked = false;
                tichuLost.IsChecked = false;
                GtichuMade.IsChecked = false;
                GtichuLost.IsChecked = false;
                tichuMade2.IsChecked = false;
                tichuLost2.IsChecked = false;
                GtichuMade2.IsChecked = false;
                GtichuLost2.IsChecked = false;
                oneTwoA.IsChecked = false;
                oneTwoB.IsChecked = false;

            }
        }
        //Edit name of Team A
        private void editTeamAbutton_Click(object sender, RoutedEventArgs e)
        {
            teamATextBlock.Visibility = Visibility.Collapsed;
            teamATextBox.Visibility = Visibility.Visible;
            editTeamAButton.Visibility = Visibility.Collapsed;
            teamASaveButton.Visibility = Visibility.Visible;
            resetTeamNamesButton.Visibility = Visibility.Collapsed;
        }
        //Save the name that you edited before
        private void teamASaveButton_Click(object sender, RoutedEventArgs e)
        {
            teamATextBox.Visibility = Visibility.Collapsed;
            teamATextBlock.Visibility = Visibility.Visible;
            editTeamAButton.Visibility = Visibility.Visible;
            teamASaveButton.Visibility = Visibility.Collapsed;
            resetTeamNamesButton.Visibility = Visibility.Visible;


            if (string.IsNullOrEmpty(teamATextBox.Text))
            {
                teamATextBlock.Text = "Team A";
            }
            else
                teamATextBlock.Text = teamATextBox.Text;
        }
        //Edit name of Team B
        private void editTeamBButton_Click(object sender, RoutedEventArgs e)
        {
            teamBTextBox.Visibility = Visibility.Visible;
            teamBTextBlock.Visibility = Visibility.Collapsed;
            editTeamBButton.Visibility = Visibility.Collapsed;
            teamBSaveButton.Visibility = Visibility.Visible;
            resetTeamNamesButton.Visibility = Visibility.Collapsed;

        }
        //Save the name that you edited before
        private void teamBSaveButton_Click(object sender, RoutedEventArgs e)
        {
            teamBTextBox.Visibility = Visibility.Collapsed;
            teamBTextBlock.Visibility = Visibility.Visible;
            editTeamBButton.Visibility = Visibility.Visible;
            teamBSaveButton.Visibility = Visibility.Collapsed;
            resetTeamNamesButton.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(teamBTextBox.Text))
            {
                teamBTextBlock.Text = "Team B";
            }
            else
                teamBTextBlock.Text = teamBTextBox.Text;
        }
        //Hamburger menu button
        private void splitViewButton_Click(object sender, RoutedEventArgs e)
        {
            if (aboutMeSplitView.IsPaneOpen)
            {
                aboutMeSplitView.IsPaneOpen = !aboutMeSplitView.IsPaneOpen;
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            }
            else
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void resetTeamsName(object sender, RoutedEventArgs e)
        {
            var resetTeams = new Windows.UI.Popups.MessageDialog("Are you sure you want to reset the names of the teams?");
            resetTeams.Commands.Add(new UICommand("Yes", null));
            resetTeams.Commands.Add(new UICommand("No", null));
            var answer = await resetTeams.ShowAsync();
            if (answer.Label == "Yes")
            {
                teamATextBlock.Text = "Team A";
                teamBTextBlock.Text = "Team B";
                teamATextBox.Text = "";
                teamBTextBox.Text = "";
            }
            else if (answer.Label == "No")
            {
                teamATextBlock.Text = teamATextBlock.Text;
                teamBTextBlock.Text = teamBTextBlock.Text;
            }
        }

        private void tipsButton(object sender, RoutedEventArgs e)
        {
            if (finalScoreA.IsChecked == true || finalScoreB.IsChecked == true)
            {
                hintButton.Visibility = Visibility.Collapsed;
                hintTextBlock.Visibility = Visibility.Collapsed;
                hintFlyout.Visibility = Visibility.Visible;
                hintFlyButtonText.Visibility = Visibility.Visible;
            }
            else
            {
                this.Frame.Navigate(typeof(FirstTimeView));
            }
        }

        private void hintFlyout_Click(object sender, RoutedEventArgs e)
        {
            hintFlyTextBlock.Text = "1)You can now enter the points of your team only\nand then it will auto complete for both teams.\n" +
                "2)If your team wins with 1-2 and tichu (or not)\nyou can check it only, you don't have to\nenter points manually.\n" +
                "3)If your team wins with tichu or grand then\n you can enter the points of your team and\ncheck the tichu (or grant) and\nit will auto complete the correct points for both teams.";
        }

        private async void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void aboutMeButton_Click(object sender, RoutedEventArgs e)
        {
            appVersionNumber.Text = string.Format("{0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);

            if (MySplitView.IsPaneOpen)
            {
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
                aboutMeSplitView.IsPaneOpen = !aboutMeSplitView.IsPaneOpen;
            }
            else
                aboutMeSplitView.IsPaneOpen = !aboutMeSplitView.IsPaneOpen;
        }

        private async void emailButton_Click(object sender, RoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=panagiotis.mirmigkis@gmail.com&subject= About Tichu Counter&body=");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        private async void facebookButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.facebook.com/giotis.mir"));
        }

        private async void twitterButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.twitter.com/panos_mir"));
        }

        private async void githubButton_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/pelopidass"));
        }

        private async void stavrosFB_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.facebook.com/StavrosXarlas"));
        }

        private async void stavrosYT_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/user/STAVROSX93"));
        }

        private async void stavrosInstagram_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://www.instagram.com/stavros_charlas/"));
        }
        //Donate Button
        private async void donationButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "";

            string business = "panagiotis.mirmigkis@gmail.com";  // your paypal email
            string description = "Enjoy%20your%20beer";            // '%20' represents a space. remember HTML!
            string country = "GR";                  // AU, US, etc.
            string currency = "EUR";                 // AUD, USD, etc.

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            await Launcher.LaunchUriAsync(new Uri(url));

        }


        //New Game button, if you want start over again
        private async void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            var newGame = new Windows.UI.Popups.MessageDialog("Do you want to start a new game?");
            newGame.Commands.Add(new UICommand("Yes", null));
            newGame.Commands.Add(new UICommand("No", null));
            var answer = await newGame.ShowAsync();
            if (answer.Label == "Yes")
            {
                teamATextBlock.Text = "Team A";
                teamBTextBlock.Text = "Team B";
                tichuMade.IsChecked = false;
                tichuLost.IsChecked = false;
                GtichuMade.IsChecked = false;
                GtichuLost.IsChecked = false;
                tichuMade2.IsChecked = false;
                tichuLost2.IsChecked = false;
                GtichuMade2.IsChecked = false;
                GtichuLost2.IsChecked = false;
                oneTwoA.IsChecked = false;
                oneTwoB.IsChecked = false;
                totalA.Text = "";
                totalB.Text = "";
                counterA = 0;
                counterB = 0;
                Scores.Clear();
                ResultListA.ItemsSource = Scores;
                ResultListB.ItemsSource = Scores;
                teamAPointsTextBox.Text = "";
                teamBPointsTextBox.Text = "";
                finalScoreA.IsChecked = false;
                finalScoreB.IsChecked = false;

                finalScoreTextBlock.Visibility = Visibility.Visible;
                finalScoreA.Visibility = Visibility.Visible;
                finalScoreB.Visibility = Visibility.Visible;
                playButton.Visibility = Visibility.Visible;
                ResultListA.Visibility = Visibility.Collapsed;
                ResultListB.Visibility = Visibility.Collapsed;
                tichuMade.Visibility = Visibility.Collapsed;
                tichuLost.Visibility = Visibility.Collapsed;
                GtichuMade.Visibility = Visibility.Collapsed;
                GtichuLost.Visibility = Visibility.Collapsed;
                tichuMade2.Visibility = Visibility.Collapsed;
                tichuLost2.Visibility = Visibility.Collapsed;
                GtichuMade2.Visibility = Visibility.Collapsed;
                GtichuLost2.Visibility = Visibility.Collapsed;
                saveButton.Visibility = Visibility.Collapsed;
                oneTwoA.Visibility = Visibility.Collapsed;
                oneTwoB.Visibility = Visibility.Collapsed;
                teamAPointsTextBox.Visibility = Visibility.Collapsed;
                teamBPointsTextBox.Visibility = Visibility.Collapsed;

                
            }
            else if (answer.Label == "No")
            {
                newGame.CancelCommandIndex = (uint)newGame.Commands.Count - 1;
            }

        }
        //LET THE GAME BEGINS! (button :P)
        private async void playGame(object sender, RoutedEventArgs e)
        {
            if (finalScoreA.IsChecked == false && finalScoreB.IsChecked == false)
            {
                var noScore = new Windows.UI.Popups.MessageDialog("You didn't set the final score.");
                await noScore.ShowAsync();
            }
            else
            {
                ResultListA.Visibility = Visibility.Visible;
                ResultListB.Visibility = Visibility.Visible;
                tichuMade.Visibility = Visibility.Visible;
                tichuLost.Visibility = Visibility.Visible;
                GtichuMade.Visibility = Visibility.Visible;
                GtichuLost.Visibility = Visibility.Visible;
                tichuMade2.Visibility = Visibility.Visible;
                tichuLost2.Visibility = Visibility.Visible;
                GtichuMade2.Visibility = Visibility.Visible;
                GtichuLost2.Visibility = Visibility.Visible;
                saveButton.Visibility = Visibility.Visible;
                oneTwoA.Visibility = Visibility.Visible;
                oneTwoB.Visibility = Visibility.Visible;
                teamATextBlock.Visibility = Visibility.Visible;
                teamBTextBlock.Visibility = Visibility.Visible;
                teamAPointsTextBox.Visibility = Visibility.Visible;
                teamBPointsTextBox.Visibility = Visibility.Visible;
                newGameButton.Visibility = Visibility.Visible;
                finalScoreTextBlock.Visibility = Visibility.Collapsed;
                playButton.Visibility = Visibility.Collapsed;
                finalScoreA.Visibility = Visibility.Collapsed;
                finalScoreB.Visibility = Visibility.Collapsed;
            }
        }
        //undo button because mistakes are part of our lifes
        private async void undoButton_Click(object sender, RoutedEventArgs e)
        {
            if (Scores.Count == 0)
            {
                hasPoints = false;
                var noEntries = new Windows.UI.Popups.MessageDialog("There is nothing to undo");
                await noEntries.ShowAsync();
            }
            else
            {
                sa = Convert.ToInt32(Scores.Last().resultA);
                sa = Int32.Parse(Scores.Last().resultA);

                sb = Convert.ToInt32(Scores.Last().resultB);
                sb = Int32.Parse(Scores.Last().resultB);

                counterA = counterA - sa;
                totalA.Text = Convert.ToString(counterA);

                counterB = counterB - sb;
                totalB.Text = Convert.ToString(counterB);

                Scores.RemoveAt(Scores.Count - 1);
                ResultListA.ItemsSource = Scores;
                ResultListB.ItemsSource = Scores;
            }
        }
    }
}
