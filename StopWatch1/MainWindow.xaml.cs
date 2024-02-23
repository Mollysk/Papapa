using System.Windows.Threading;
using System.Diagnostics;
using System.Windows;
using System;
using System.Windows.Media;

namespace StopWatch1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members
        // Initialize the timer for stopwatch
        private DispatcherTimer timerForStopwatch = new DispatcherTimer();

        // Initialize the timer for timer
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        // Initializing the Time Measurement
        private Stopwatch stopwatch = new Stopwatch();

        // We initialize the time interval for stopwatch
        private TimeSpan elapsedTime = TimeSpan.Zero;

        // We initialize the time interval for timer
        private TimeSpan timerTime;

        // Check when stopwatch starts
        private bool isRunning = false;

        // Conventional counter
        private int counter = 1;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            InitializeStopwatch();
            InitializeTimer();
        }
        #endregion

        #region Stopwatch

        #region InitializeStopwatch
        private void InitializeStopwatch()
        {
            // Update time every 10 milliseconds
            timerForStopwatch.Interval = TimeSpan.FromMilliseconds(10);
            timerForStopwatch.Tick += TimerTick;
        }
        #endregion

        #region TimerTick
        private void TimerTick(object sender, EventArgs s)
        {
            // Getting elapsed time from Stopwatch
            elapsedTime = stopwatch.Elapsed;

            // Updating TextBlock
            timeTextBlock.Text = string.Format("{0:mm\\:ss\\.ff}", elapsedTime);
        }
        #endregion

        #region StartStopButton
        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                // Start the countdown by pressing the "Start" button
                stopwatch.Start();
                timerForStopwatch.Start();

                // Change button text to "Stop"
                startStopButton.Content = "Stop";

                // Change button background color to "Red"
                startStopButton.Background = Brushes.Red;

                // Change button text to "Lap"
                saveResetButton.Content = "Lap";
            }
            else
            {
                // Stop the countdown by pressing the "Start" button again
                stopwatch.Stop();
                timerForStopwatch.Stop();

                // Change button text to "Start"
                startStopButton.Content = "Start";

                // Change button background color to "Green"
                startStopButton.Background = Brushes.Green;

                // Change button text to "Reset"
                saveResetButton.Content = "Reset";
            }

            isRunning = !isRunning; // Инвертируем состояние секундомера
        }
        #endregion

        #region SaveResetTimeButton
        private void SaveResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                // Store current time in ListBox
                savedTimesListBox.Items.Insert(0, string.Format("Lap {0}: {1:mm\\:ss\\.ff}", counter, elapsedTime));

                // Counter for writing time on the account
                counter++;

            }
            else
            {
                // Reset the timer time value to initial
                stopwatch.Reset();

                // Reset counter
                counter = 1;

                // Set the start time
                timeTextBlock.Text = "00:00.00";

                // Clearing the list of saved times
                savedTimesListBox.Items.Clear();
            }
        }
        #endregion 

        #region SwitchToTimer
        private void SwitchToTimer_Click(object sender, RoutedEventArgs e)
        {
            // Hide the stopwatch container
            stopwatchGrid.Visibility = Visibility.Collapsed;

            // Showing a container with a timer
            timerGrid.Visibility = Visibility.Visible;
        }
        #endregion

        #endregion

        #region Timer

        #region InitializeTimer
        private void InitializeTimer()
        {
            // Attach the Timer_Tick method as an event handler for the Tick event of the dispatcherTimer.
            dispatcherTimer.Tick += Timer_Tick;
            // Set the interval of the dispatcherTimer to one second.
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);

            // Set the initial time of the timer
            timerTime = TimeSpan.FromSeconds(0);
            UpdateTimerDisplay();
        }
        #endregion

        #region Timer_Tick
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timerTime.TotalSeconds > 0)
            {
                // If there is remaining time, subtract one second from the timerTime variable.
                timerTime = timerTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerDisplay();
            }
            else
            {
                dispatcherTimer.Stop();
                MessageBox.Show("Time is over!");
            }
        }
        #endregion

        #region UpdateTimerDisplay
        private void UpdateTimerDisplay()
        {
            //Write timerTimeTamble in the correct format
            timerTimeTable.Text = timerTime.ToString(@"hh\:mm\:ss");
        }
        #endregion

        #region StartTimer
        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(timerInputTextBox.Text, out int timerSeconds))
            {
                // Convert entered time from seconds to TimeSpan
                timerTime = TimeSpan.FromSeconds(timerSeconds);

                // Updating the time display
                UpdateTimerDisplay();

                // Start a timer
                dispatcherTimer.Start();
            }
            else
            {
                MessageBox.Show("Please enter the correct time in seconds.");
            }
        }
        #endregion

        #region CancelTimer
        private void CancelTimer_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            timerTimeTable.Text = "00:00:00";
        }
        #endregion

        #region SwitchToStopWatch
        private void SwitchToStopWatch_Click(object sender, RoutedEventArgs e)
        {
            // Hiding the stopwatch container
            stopwatchGrid.Visibility = Visibility.Visible;

            // Showing a container with a timer
            timerGrid.Visibility = Visibility.Collapsed;
        }
        #endregion

        #endregion
    }
}
