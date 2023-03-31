using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DrayTek_WAN_Status {
    public class WanStatus {
        public decimal UpSpeed {
            get {
                if (!_upSpeed.HasValue) {
                    var matchUpSpeed = Regex.Match(_rawContent, @"((?<=UpSpeed=)|(?<=UP Speed:))\d*");
                    if (!matchUpSpeed.Success) { return 0; }
                    _upSpeed = Math.Round(Decimal.Parse(matchUpSpeed.Value) / 1000 / 1000, 2);
                }
                return _upSpeed.Value;
            }
        }

        public decimal DownSpeed {
            get {
                if (!_downSpeed.HasValue) {
                    var matchDownSpeed = Regex.Match(_rawContent, @"((?<=DownSpeed=)|(?<=Down Speed:))\d*");
                    if (!matchDownSpeed.Success) { return 0; }
                    _downSpeed = Math.Round(Decimal.Parse(matchDownSpeed.Value) / 1000 / 1000, 2);
                }
                return _downSpeed.Value;
            }
        }

        public decimal CorrectedBlocks
        {
            get
            {
                if (!_correctedBlocks.HasValue)
                {
                    var matchCorrectedBlocks = Regex.Match(_rawContent, @"((?<=Corrected Blocks=)|(?<=Corrected Blocks:))\d*");
                    if (!matchCorrectedBlocks.Success) { return 0; }
                    _correctedBlocks = Math.Round(Decimal.Parse(matchCorrectedBlocks.Value), 0);
                }
                return _correctedBlocks.Value;
            }
        }
        public decimal UncorrectedBlocks
        {
            get
            {
                if (!_uncorrectedBlocks.HasValue)
                {
                    var matchUncorrectedBlocks = Regex.Match(_rawContent, @"((?<=Uncorrected Blocks=)|(?<=Uncorrected Blocks:))\d*");
                    if (!matchUncorrectedBlocks.Success) { return 0; }
                    _uncorrectedBlocks = Math.Round(Decimal.Parse(matchUncorrectedBlocks.Value), 0);
                }
                return _uncorrectedBlocks.Value;
            }
        }
        public decimal SnrMargin
        {
            get
            {
                if (!_snrMargin.HasValue)
                {
                    var matchUnrMargin = Regex.Match(_rawContent, @"((?<=SNR Margin=)|(?<=SNR Margin:))\d*");
                    if (!matchUnrMargin.Success) { return 0; }
                    _snrMargin = Math.Round(Decimal.Parse(matchUnrMargin.Value), 1);
                }
                return _snrMargin.Value;
            }
        }
        public decimal LoopAtt
        {
            get
            {
                if (!_loopAtt.HasValue)
                {
                    var matchLoopAtt = Regex.Match(_rawContent, @"((?<=Loop Att.=)|(?<=Loop Att.:))\d*");
                    if (!matchLoopAtt.Success) { return 0; }
                    _loopAtt = Math.Round(Decimal.Parse(matchLoopAtt.Value), 1);
                }
                return _loopAtt.Value;
            }
        }
        public DateTime Timestamp {
            get {
                if (!_timestamp.HasValue) {
                    var matchDate = Regex.Match(_rawContent, "(?<=>).*(?= Vigor)");
                    if (!matchDate.Success) { return DateTime.Now; }
                    var value = matchDate.Value.Replace("  ", " "); // days smaller then 10 will create an additional space
                    _timestamp = DateTime.ParseExact(value, "MMM d HH:mm:ss", CultureInfo.InvariantCulture);
                }
                return _timestamp.Value;
            }
        }

        public bool IsConnected {
            get {
                if (!_isConnected.HasValue) {
                    _isConnected = _rawContent.Contains("SHOWTIME");
                }
                return _isConnected.Value;
            }
        }
  
        public string SpeedUnit { get { return "Mbps"; } }

        private bool? _isConnected = null;
        private DateTime? _timestamp = null;
        private decimal? _downSpeed = null;
        private decimal? _upSpeed = null;

        private decimal? _correctedBlocks = null;  // Corrected Blocks:
        private decimal? _uncorrectedBlocks = null;  // Uncorrected Blocks:
        private decimal? _snrMargin = null;  // SNR Margin:
        private decimal? _loopAtt = null;  // Loop Att.:
        private string _rawContent;

        // UDP Data:
        // <174>Jan 11 05:34:03 Vigor: ADSL_Status:[Mode=24A States=SHOWTIME UpSpeed=26996000 DownSpeed=59815000 SNR=9 Atten=17 ]

        // Telnet Data
        // VDSL Information:      VDSL Firmware Version:05-06-07-06-01-07
        // Mode:17A State:SHOWTIME TX Block:0     RX Block:0
        // Corrected Blocks:31    Uncorrected Blocks:0
        // UP Speed:26996000   Down Speed:59815000   SNR Margin:9   Loop Att.:17

        // jac
        // VDSL Information:      VDSL Firmware Version:05-07-06-0D-01-07
        // Mode:17A State:SHOWTIME TX Block:0     RX Block:0
        // Corrected Blocks:3905  Uncorrected Blocks:32
        // UP Speed:20000000   Down Speed:79263000   SNR Margin:5   Loop Att.:6

        public static WanStatus Parse(string content) {
            return ContainsWanStatus(content) ? GetStatus(content) : new WanStatus();
        }

        public static bool ContainsWanStatus(string content) {
            return content.Contains("ADSL_Status") || content.Contains("VDSL Information");
        }

        private static WanStatus GetStatus(string content) {
            return new WanStatus {
                _rawContent = content
            };
        }
    }
}
