using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace OBS_Overlay_Manager.Models
{
    internal class PNGComposite
    {
        private const int W = 1920;
        private const int H = 1080;

        private readonly Font _teamNameFont = new Font("Rajdhani", 36, FontStyle.Bold);
        private readonly Font _scoreFont = new Font("Rajdhani", 50, FontStyle.Bold);
        private readonly Bitmap _clear = new(W, H);

        private readonly Bitmap _blueLeft;
        private readonly Bitmap _goldLeft;
        private readonly Bitmap _greenLeft;
        private readonly Bitmap _purpleLeft;
        private readonly Bitmap _redLeft;
        private readonly Bitmap _blueRight;
        private readonly Bitmap _goldRight;
        private readonly Bitmap _greenRight;
        private readonly Bitmap _purpleRight;
        private readonly Bitmap _redRight;
        private readonly Bitmap _topMid;

        private Bitmap? _comp;
        private Graphics? _graphics;

        private readonly string _mainDir;

        public PNGComposite()
        {
            _mainDir = "Resources\\Overlays\\Main";
#if DEBUG
            _mainDir = $"..\\..\\..\\{_mainDir}";
#endif
            string dirLeft = $"{_mainDir}\\Left";
            string dirRight = $"{_mainDir}\\Right";
            string fileTopMid = $"{_mainDir}\\Top_Mid.png";

            _blueLeft = new($"{dirLeft}\\Blue.png");
            _goldLeft = new($"{dirLeft}\\Gold.png");
            _greenLeft = new($"{dirLeft}\\Green.png");
            _purpleLeft = new($"{dirLeft}\\Purple.png");
            _redLeft = new($"{dirLeft}\\Red.png");
            _blueRight = new($"{dirRight}\\Blue.png");
            _goldRight = new($"{dirRight}\\Gold.png");
            _greenRight = new($"{dirRight}\\Green.png");
            _purpleRight = new($"{dirRight}\\Purple.png");
            _redRight = new($"{dirRight}\\Red.png");
            _topMid = new(fileTopMid);

            //hide overlays on launch
            Update(null, null, false);
        }

        public void Update(Profile profileLeft, Profile profileRight, bool showOverlays)
        {
            _comp = new(W, H);
            _graphics = Graphics.FromImage(_comp);

            if (showOverlays)
            {
                Bitmap overlayLeft = profileLeft.Color switch
                {
                    0 => _blueLeft,
                    1 => _goldLeft,
                    2 => _greenLeft,
                    3 => _purpleLeft,
                    4 => _redLeft,
                    _ => _clear,
                };
                Bitmap overlayRight = profileRight.Color switch
                {
                    0 => _blueRight,
                    1 => _goldRight,
                    2 => _greenRight,
                    3 => _purpleRight,
                    4 => _redRight,
                    _ => _clear,
                };
                _graphics.DrawImage(overlayLeft, 0, 0, W, H);
                _graphics.DrawImage(overlayRight, 0, 0, W, H);
                _graphics.DrawImage(_topMid, 0, 0, W, H);


                SizeF teamNameLeftSize = _graphics.MeasureString(profileLeft.TeamName, _teamNameFont);
                _graphics.DrawString(profileLeft.TeamName, _teamNameFont, Brushes.White, CenterTeamName(teamNameLeftSize, true));

                SizeF teamNameRightSize = _graphics.MeasureString(profileRight.TeamName, _teamNameFont);
                _graphics.DrawString(profileRight.TeamName, _teamNameFont, Brushes.White, CenterTeamName(teamNameRightSize, false));

                SizeF scoreLeftSize = _graphics.MeasureString(profileLeft.Score.ToString(), _scoreFont);
                _graphics.DrawString(profileLeft.Score.ToString(), _scoreFont, Brushes.White,CenterScore(scoreLeftSize, true));

                SizeF scoreRightSize = _graphics.MeasureString(profileRight.Score.ToString(), _scoreFont);
                _graphics.DrawString(profileRight.Score.ToString(), _scoreFont, Brushes.White, CenterScore(scoreRightSize, false));
            }
            else
            {
                _graphics.CompositingMode = CompositingMode.SourceCopy;
                _graphics.DrawImage(_clear, 0, 0, W, H);
                _graphics.CompositingMode = CompositingMode.SourceOver;
            }

            _comp.Save($"{_mainDir}\\Composite.png", ImageFormat.Png);
            _comp.Dispose();
            _graphics.Dispose();
        }

        private static PointF CenterTeamName(SizeF textSize, bool isLeft)
        {
            if (isLeft)
            {
                float x = 378 - (textSize.Width / 2);
                float y = 52 - (textSize.Height / 2);
                return new PointF(x, y);
            }
            else
            {
                float x = 1542 - (textSize.Width / 2);
                float y = 52 - (textSize.Height / 2);
                return new PointF(x, y);
            }
        }

        private static PointF CenterScore(SizeF textSize, bool isLeft)
        {
            if (isLeft)
            {
                float x = 764 - (textSize.Width / 2);
                float y = 56 - (textSize.Height / 2);
                return new PointF(x, y);
            }
            else
            {
                float x = 1156 - (textSize.Width / 2);
                float y = 56 - (textSize.Height / 2);
                return new PointF(x, y);
            }
        }
    }
}
