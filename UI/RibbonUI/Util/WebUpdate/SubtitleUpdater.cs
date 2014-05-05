using System;
using System.Threading.Tasks;
using RibbonUI.UserControls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Util.WebUpdate {
    public class SubtitleUpdater {
        private readonly ObservableMovie _movie;
        private readonly SubtitleSites _subSite;

        public SubtitleUpdater(ObservableMovie movie, SubtitleSites subSite) {
            _movie = movie;
            _subSite = subSite;
        }

        public async Task Update() {
            switch (_subSite) {
                case SubtitleSites.PodnapisiNet:
                    await UpdatePodnapisiNet();
                    break;
                case SubtitleSites.OpenSubtitles:
                    await UpdateOpenSubtitles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async Task UpdateOpenSubtitles() {

            return;
        }

        private async Task UpdatePodnapisiNet() {
            return;
        }
    }
}
