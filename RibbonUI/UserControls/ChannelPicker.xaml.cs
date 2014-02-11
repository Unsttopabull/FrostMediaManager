using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Frost.Common;
using Frost.Common.Annotations;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI.UserControls {

    public enum Channel {
        Unknown,
        FrontCenter,
        FrontRight,
        FrontLeft,
        SideRight,
        SideLeft,
        BackRight,
        BackLeft,
        LFE
    }

    /// <summary>Interaction logic for ChannelPicker.xaml</summary>
    public partial class ChannelPicker : UserControl, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty ChannelSetupProperty = DependencyProperty.Register("ChannelSetup", typeof(string), typeof(ChannelPicker),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ChannelLayoutProperty = DependencyProperty.Register("ChannelLayout", typeof(string), typeof(ChannelPicker),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ChannelPositionsProperty = DependencyProperty.Register("ChannelPositions", typeof(string), typeof(ChannelPicker),
            new PropertyMetadata(default(string), OnChannelPositionsChanged));

        public static readonly DependencyProperty NumberOfChannelsProperty = DependencyProperty.Register("NumberOfChannels", typeof(int?), typeof(ChannelPicker),
            new PropertyMetadata(default(int?), OnNumberOfChannelChanged));

        private readonly HashSet<Channel> _front;
        private readonly HashSet<Channel> _side;
        private readonly HashSet<Channel> _back;

        public ChannelPicker() {
            InitializeComponent();

            _front = new HashSet<Channel>();
            _side = new HashSet<Channel>();
            _back = new HashSet<Channel>();
        }

        public string ChannelSetup {
            get { return (string) GetValue(ChannelSetupProperty); }
            set { SetValue(ChannelSetupProperty, value); }
        }

        public string ChannelLayout {
            get { return (string) GetValue(ChannelLayoutProperty); }
            set { SetValue(ChannelLayoutProperty, value); }
        }

        public string ChannelPositions {
            get { return (string) GetValue(ChannelPositionsProperty); }
            set { SetValue(ChannelPositionsProperty, value); }
        }

        public int? NumberOfChannels {
            get { return (int?) GetValue(NumberOfChannelsProperty); }
            set { SetValue(NumberOfChannelsProperty, value); }
        }

        private static void OnChannelPositionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ChannelPicker) d).SetFromChannelPositionsString((string) e.NewValue);
        }

        private static void OnNumberOfChannelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ChannelPicker) d).SetFromNumberOfChannels((int?) e.NewValue);
        }

        private void SetFromNumberOfChannels(int? numberOfChannels) {
            if (!numberOfChannels.HasValue || !string.IsNullOrEmpty(ChannelPositions)) {
                return;
            }

            if (numberOfChannels == 2) {
                _front.Add(Channel.FrontLeft);
                _front.Add(Channel.FrontRight);

                FrontRight.IsChecked = true;
                FrontLeft.IsChecked = true;

                return;
            }

            if (numberOfChannels == 6) {
                _front.Add(Channel.FrontLeft);
                _front.Add(Channel.FrontRight);
                _front.Add(Channel.FrontCenter);

                FrontCenter.IsChecked = true;
                FrontRight.IsChecked = true;
                FrontLeft.IsChecked = true;

                _side.Add(Channel.SideLeft);
                _side.Add(Channel.SideRight);

                SideRight.IsChecked = true;
                SideLeft.IsChecked = true;

                Woofer.IsChecked = true;
            }

            if (numberOfChannels == 8) {
                _front.Add(Channel.FrontLeft);
                _front.Add(Channel.FrontRight);
                _front.Add(Channel.FrontCenter);

                FrontCenter.IsChecked = true;
                FrontRight.IsChecked = true;
                FrontLeft.IsChecked = true;

                _side.Add(Channel.SideLeft);
                _side.Add(Channel.SideRight);

                SideRight.IsChecked = true;
                SideLeft.IsChecked = true;

                _back.Add(Channel.BackLeft);
                _back.Add(Channel.BackRight);

                BackRight.IsChecked = true;
                BackLeft.IsChecked = true;

                Woofer.IsChecked = true;
            }

            GetChannelPositions();
            GetChannelSetup();
            ChannelLayout = Woofer.IsChecked == true
                                ? (NumberOfChannels - 1) + ".1"
                                : NumberOfChannels + ".0";
        }

        private void SetFromChannelPositionsString(string channelPositions) {
            if (string.IsNullOrEmpty(channelPositions)) {
                return;
            }

            foreach (ToggleButton toggle in LogicalTreeHelper.GetChildren(this).OfType<ToggleButton>()) {
                toggle.IsChecked = false;
            }

            int numChannels = 0;
            string[] speakerPositions = channelPositions.SplitWithoutEmptyEntries(", ");
            foreach (string position in speakerPositions) {
                int front = position.IndexOf("Front:", StringComparison.InvariantCultureIgnoreCase);
                if (front != -1) {
                    string[] speakers = position.Remove(front, 6).SplitWithoutEmptyEntries(" ");
                    foreach (string speaker in speakers) {
                        switch (speaker) {
                            case "C":
                                numChannels++;
                                _front.Add(Channel.FrontCenter);
                                FrontCenter.IsChecked = true;
                                break;
                            case "L":
                                numChannels++;
                                _front.Add(Channel.FrontLeft);
                                FrontLeft.IsChecked = true;
                                break;
                            case "R":
                                numChannels++;
                                _front.Add(Channel.FrontRight);
                                FrontRight.IsChecked = true;
                                break;
                        }
                    }

                    continue;
                }

                int side = position.IndexOf("Side:", StringComparison.InvariantCultureIgnoreCase);
                if (side != -1) {
                    string[] speakers = position.Remove(side, 5).SplitWithoutEmptyEntries(" ");

                    foreach (string speaker in speakers) {
                        switch (speaker) {
                            case "L":
                                numChannels++;
                                _side.Add(Channel.SideLeft);
                                SideLeft.IsChecked = true;
                                break;
                            case "R":
                                numChannels++;
                                _side.Add(Channel.SideRight);
                                SideRight.IsChecked = true;
                                break;
                        }
                    }
                    continue;
                }

                int back = position.IndexOf("Back:", StringComparison.InvariantCultureIgnoreCase);
                if (back != -1) {
                    string[] speakers = position.Remove(back, 5).SplitWithoutEmptyEntries(" ");

                    foreach (string speaker in speakers) {
                        switch (speaker) {
                            case "L":
                                numChannels++;
                                _back.Add(Channel.BackLeft);
                                BackLeft.IsChecked = true;
                                break;
                            case "R":
                                numChannels++;
                                _back.Add(Channel.BackRight);
                                SideRight.IsChecked = true;
                                break;
                        }
                    }
                    continue;
                }

                if (position.Equals("LFE", StringComparison.InvariantCultureIgnoreCase)) {
                    Woofer.IsChecked = true;
                    numChannels++;
                }
            }

            NumberOfChannels = numChannels;
            GetChannelSetup();
            ChannelLayout = Woofer.IsChecked == true
                                ? (NumberOfChannels - 1) + ".1"
                                : NumberOfChannels + ".0";
        }

        private void OnToggleButtonClicked(object sender, RoutedEventArgs e) {
            ToggleButton source = (ToggleButton) sender;
            Channel channels = (Channel) source.Tag;

            if (source.IsChecked == true) {
                AddRemove(false, channels);
                NumberOfChannels++;
            }
            else {
                AddRemove(true, channels);
                NumberOfChannels--;
            }

            GetChannelPositions();
            GetChannelSetup();

            ChannelLayout = Woofer.IsChecked == true
                                ? (NumberOfChannels - 1) + ".1"
                                : NumberOfChannels + ".0";
        }

        private void GetChannelSetup() {
            StringBuilder sb = new StringBuilder();
            if (_front.Count > 0) {
                sb.Append(_front.Count);
            }

            if (_side.Count > 0) {
                sb.Append("/" + _side.Count);
            }

            if (_back.Count > 0) {
                sb.Append("/" + _back.Count);
            }
            ChannelSetup = sb.ToString();
        }

        private void GetChannelPositions() {
            StringBuilder sb = new StringBuilder();
            if (_front.Count > 0) {
                sb.Append("Front:");

                foreach (Channel channel in _front) {
                    switch (channel) {
                        case Channel.FrontCenter:
                            sb.Append(" C");
                            break;
                        case Channel.FrontRight:
                            sb.Append(" R");
                            break;
                        case Channel.FrontLeft:
                            sb.Append(" L");
                            break;
                    }
                }
            }

            if (_side.Count > 0) {
                sb.Append(", Side:");

                foreach (Channel channel in _side) {
                    switch (channel) {
                        case Channel.SideRight:
                            sb.Append(" R");
                            break;
                        case Channel.SideLeft:
                            sb.Append(" L");
                            break;
                    }
                }
            }

            if (_back.Count > 0) {
                sb.Append(", Back:");

                foreach (Channel channel in _back) {
                    switch (channel) {
                        case Channel.BackRight:
                            sb.Append(" R");
                            break;
                        case Channel.BackLeft:
                            sb.Append(" L");
                            break;
                    }
                }
            }

            if (Woofer.IsChecked == true) {
                sb.Append(", LFE");
            }
            ChannelPositions = sb.ToString();
        }

        private bool AddRemove(bool remove, Channel channel) {
            if (channel != Channel.LFE) {
                HashSet<Channel> hs = null;
                switch (channel) {
                    case Channel.FrontLeft:
                    case Channel.FrontCenter:
                    case Channel.FrontRight:
                        hs = _front;
                        break;
                    case Channel.SideLeft:
                    case Channel.SideRight:
                        hs = _side;
                        break;
                    case Channel.BackLeft:
                    case Channel.BackRight:
                        hs = _back;
                        break;
                }

                if (hs == null) {
                    return false;
                }

                return remove ? hs.Remove(channel) : hs.Add(channel);
            }
            return true;
        }
    }

}