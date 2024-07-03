using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Pomodoro_Timer
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        private bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            UpdatePlayPauseButton();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            double minutes = 0, seconds = 0;

            if (!string.IsNullOrWhiteSpace(HoursInputActual.Text))
            {
                if (!double.TryParse(HoursInputActual.Text, out minutes))
                {
                    MessageBox.Show("Please enter a valid number for minutes.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(MinutesInputActual.Text))
            {
                if (!double.TryParse(MinutesInputActual.Text, out seconds))
                {
                    MessageBox.Show("Please enter a valid number for seconds.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            HandleStartTimer(minutes, seconds);
        }

        private void HandleStartTimer(double minutes, double seconds)
        {
            try
            {
                int totalSeconds = (int)((minutes * 60) + seconds);

                if (totalSeconds > 0)
                {
                    remainingTime = TimeSpan.FromSeconds(totalSeconds);
                    UpdateTimerDisplay();
                    Panel1.Visibility = Visibility.Collapsed;
                    Panel2.Visibility = Visibility.Visible;

                    UpdatePlayPauseButton();
                    ResetButton.Visibility = Visibility.Visible;

                    StartTimer();
                }
                else
                {
                    MessageBox.Show("Please enter a time greater than 0 seconds.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isRunning)
                {
                    PauseTimer();
                }
                else
                {
                    StartTimer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox == HoursInputActual && HoursInput.Text == "MM")
                {
                    HoursInput.Text = "";
                }
                else if (textBox == MinutesInputActual && MinutesInput.Text == "SS")
                {
                    MinutesInput.Text = "";
                }
            }
        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if (textBox == HoursInputActual && string.IsNullOrWhiteSpace(HoursInput.Text))
                {
                    HoursInput.Text = "MM";
                }
                else if (textBox == MinutesInputActual && string.IsNullOrWhiteSpace(MinutesInput.Text))
                {
                    MinutesInput.Text = "SS";
                }
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PauseTimer();
                Panel2.Visibility = Visibility.Collapsed;
                Panel1.Visibility = Visibility.Visible;
                HoursInputActual.Text = "";
                MinutesInputActual.Text = "";
                HoursInput.Visibility = Visibility.Visible;
                MinutesInput.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartTimer()
        {
            timer.Start();
            isRunning = true;
            UpdatePlayPauseButton();
        }

        private void PauseTimer()
        {
            timer.Stop();
            isRunning = false;
            UpdatePlayPauseButton();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (remainingTime.TotalSeconds > 0)
                {
                    remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                    UpdateTimerDisplay();
                }
                else
                {
                    PauseTimer();
                    MessageBox.Show("Timer finished!", "Time's up", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTimerDisplay()
        {
            TimerDisplay.Text = remainingTime.ToString(@"hh\:mm\:ss");
        }

        private void UpdatePlayPauseButton()
        {
            try
            {
                if (PlayPauseButton.Template.FindName("PlayPauseIcon", PlayPauseButton) is Path playPauseIcon)
                {
                    playPauseIcon.Data = isRunning
                        ? Geometry.Parse("M5,5 L10,5 L10,19 L5,19 Z M14,5 L19,5 L19,19 L14,19 Z")
                        : Geometry.Parse("M8,5 L8,19 L19,12 Z");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the play/pause button: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}