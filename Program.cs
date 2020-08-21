using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ScoutPatrolPoster
{

    public class Spejder
    {
        public string name;
        public string PathToImage;
    }

    public class Patrulje 
    {
        public Patrulje()
        {
            spejder = new List<Spejder>();
        }

        public List<Spejder> spejder;
        public string name;
    }

    public class Green
    {
        public Green()
        {
            Leder = new List<Spejder>();
            patrulje = new List<Patrulje>();
        }

        public string name;
        public List<Spejder> Leder;
        public List<Patrulje> patrulje;

    }

    class Program
    {
        private static Green LoadData()
        {
            string path = @"C:\Users\kasper steenstrup\Desktop\Valpe";

            var leaderPaths = Directory.GetFiles(path, "*.png", SearchOption.TopDirectoryOnly).ToList();
            var patruljePaths = Directory.GetDirectories(path).ToList();

            var green = new Green
            {
                name = Path.GetFileNameWithoutExtension(path)
            };

            foreach (var leader in leaderPaths)
            {
                green.Leder.Add(new Spejder() { PathToImage = leader, name = Path.GetFileNameWithoutExtension(leader) });
            }

            foreach (var patruljePath in patruljePaths)
            {
                var patrulje = new Patrulje();
                patrulje.name = Path.GetDirectoryName(patruljePath);
                var spejders = Directory.GetFiles(patruljePath, "*.png", SearchOption.TopDirectoryOnly).ToList();
                foreach (var spejder in spejders)
                {
                    patrulje.spejder.Add(new Spejder() { PathToImage = spejder, name = Path.GetFileNameWithoutExtension(spejder) });
                }
                green.patrulje.Add(patrulje);
            }
            return green;
        }

        private static void GreenToPoster(Green green)
        {
          var Poster = new Bitmap(842, 1191);


            // set all to white
            
            for (var x = 0; x < Poster.Width; x++)
                {
                    for (var y = 0; y < Poster.Height; y++)
                    {
                        Poster.SetPixel(x, y, Color.White);
                    }
                }

            // add leders
            for (int i = 0; i < green.Leder.Count; i++)
            {
                Image image = Image.FromFile(green.Leder[i].PathToImage);

                using (Graphics g = Graphics.FromImage(Poster))
                {
                    g.DrawImage(image, 10+i*170, 10, 150, 150);
                    g.DrawString(green.Leder[i].name, new Font("Arial", 30), Brushes.Black, 10 + i * 170, 160);
                }
            }

            // add scouts

            for (int j = 0; j < green.patrulje.Count; j++)
            {
                using (Graphics g = Graphics.FromImage(Poster))
                {
                    //g.DrawString(green.patrulje[j].name, new Font("Arial", 35), Brushes.Black, 10, 250 * (j + 1 ));
                }

                for (int i = 0; i < green.patrulje[j].spejder.Count; i++)
                {
                    Image image = Image.FromFile(green.patrulje[j].spejder[i].PathToImage);

                    using (Graphics g = Graphics.FromImage(Poster))
                    {
                        g.DrawImage(image, 10 + i * 195, 240 + 240*j, 150, 150);
                        g.DrawString(green.patrulje[j].spejder[i].name, new Font("Arial", 35), Brushes.Black, 10 + i * 195, 390 + 240 * j);
                    }
                }
            }
            string path = @"C:\Users\kasper steenstrup\Desktop\Valpe.png";
            Poster.Save(path);

        }



        static void Main(string[] args)
        {

            var green = LoadData();

            GreenToPoster(green);
        }
    }
}
