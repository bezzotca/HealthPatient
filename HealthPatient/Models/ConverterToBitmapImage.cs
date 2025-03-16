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
        public static Bitmap ConvertToDoctor(string Image, int RoleId)
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
    }
}
