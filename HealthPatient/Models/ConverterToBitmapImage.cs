using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HealthPatient.Models
{
    public partial class ConverterToBitmapImage
    {
        public static Bitmap ConvertToDoctor(string Image, int? RoleId)
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            Uri uri = new Uri($"avares://{assemblyName}/Assets/Media/Doctors_media/{Image}");
            if(AssetLoader.Exists(uri))
            {
                return new Bitmap(AssetLoader.Open(uri));
            }
            else if(!AssetLoader.Exists(uri))
            {
                switch (RoleId)
                {
                    case 1:
                        Uri uridefman = new Uri($"avares://{assemblyName}/Assets/DefaultMan.jfif");
                        return new Bitmap(AssetLoader.Open(uridefman));
                    case 2:
                        Uri uridefgirl = new Uri($"avares://{assemblyName}/Assets/DefaultGirl.jpg");
                        return new Bitmap(AssetLoader.Open(uridefgirl));
                }
            }
            Uri uridefma = new Uri($"avares://{assemblyName}/Assets/DefaultMan.jfif");
            return new Bitmap(AssetLoader.Open(uridefma));
        }
        public static Bitmap ConvertToAchieve(string Image, int? AchievementId)
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            if(AchievementId != null)
            {
                switch (AchievementId)
                {
                    case 1:
                        Uri uri1 = new Uri($"avares://{assemblyName}/Assets/Media/Achievements_media/{Image}");
                        return new Bitmap(AssetLoader.Open(uri1));
                    case 2:
                        Uri uri2 = new Uri($"avares://{assemblyName}/Assets/Media/Achievements_media/{Image}");
                        return new Bitmap(AssetLoader.Open(uri2));
                    case 3:
                        Uri uri3 = new Uri($"avares://{assemblyName}/Assets/Media/Achievements_media/{Image}");
                        return new Bitmap(AssetLoader.Open(uri3));
                    case 4:
                        Uri uri4 = new Uri($"avares://{assemblyName}/Assets/Media/Achievements_media/{Image}");
                        return new Bitmap(AssetLoader.Open(uri4));
                    case 5:
                        Uri uri5 = new Uri($"avares://{assemblyName}/Assets/Media/Achievements_media/{Image}");
                        return new Bitmap(AssetLoader.Open(uri5));
                    default:
                        Uri uridef = new Uri($"avares://{assemblyName}/Assets/NoneAchieve.png");
                        return new Bitmap(AssetLoader.Open(uridef));
                }
            }
            else
            {
                Uri uridef = new Uri($"avares://{assemblyName}/Assets/NoneAchieve.png");
                return new Bitmap(AssetLoader.Open(uridef));
            }
            
            
        }
    }
}
