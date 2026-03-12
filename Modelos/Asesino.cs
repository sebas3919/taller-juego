using Animaciones.Componentes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Animaciones.Modelos
{
    internal class Asesino : Personaje
    {
        public Animator animator = new Animator();
        public enum EstadosAsesino
        {
            Idle,
            CaminandoDerecha,
            CaminandoIzquierda,
            AtacandoDerecha,
        }
        int Frame = 0;
        public EstadosAsesino EstadoActual;
        public Asesino()
        {
            Skin = new PictureBox();
            string rutaImagen = Path.Combine(Application.StartupPath, "Animaciones", "Asesino", "Right - Walking", "Right - Walking_000.png");
            Skin.Image = Image.FromFile(rutaImagen);
            animator = new Animator();
            animator.personaje = this;

            var idle = new Animacion()
            {
                Name = "Front - Idle",
                frames = LeerImagenes("Front - Idle"),
                framerate = 1
            };

            var walkLeft = new Animacion()
            {
                Name = "Left - Walking",
                frames = LeerImagenes("Left - Walking"),
                framerate = 1
            };

            var walkRight = new Animacion()
            {
                Name = "Right - Walking",
                frames = LeerImagenes("Right - Walking"),
                framerate = 1
            };

            var attackRight = new Animacion()
            {
                Name = "Right - Attacking",
                frames = LeerImagenes("Right - Attacking"),
                framerate = 1
            };

            animator.AddAnimation(idle);
            animator.AddAnimation(walkLeft);
            animator.AddAnimation(walkRight);
            animator.AddAnimation(attackRight);

            animator.Play("Front - Idle");

            // Skin.SizeMode = PictureBoxSizeMode.StretchImage;
            Skin.Width = 100;
            Skin.Height = 100;
            Skin.Location = new Point(100, 100);
        }

        public void ActualizarPantalla()
        {
            //animator.Update();
            switch (EstadoActual)
            {
                case EstadosAsesino.CaminandoDerecha:
                    animator.Play("Right - Walking");
                    break;

                case EstadosAsesino.CaminandoIzquierda:
                    animator.Play("Left - Walking");
                    break;
                case EstadosAsesino.AtacandoDerecha:
                    animator.Play("Right - Attacking");
                    break;
                case EstadosAsesino.Idle:
                    animator.Play("Front - Idle");
                    Console.WriteLine("Idle");
                    break;

            }
        }

        public void Saltar()
        {

            Skin.Top -= 1;
        }

        public List<Image> LeerImagenes(string prefijo)
        {

            /*var imagen = $"{prefijo}_00{Frame}.png";
            if (Frame > 9)
            {
                imagen = $"{prefijo}_0{Frame}.png";
            }*/
            var frames = new List<Image>();
            string rutaImagen = Path.Combine(
               Application.StartupPath,
               "Animaciones",
               "Asesino",
               prefijo);

            foreach (var file in Directory.GetFiles(rutaImagen))
            {
                using (var imgTemp = Image.FromFile(file))
                {
                    frames.Add(new Bitmap(imgTemp)); // 🔥 clona y libera el archivo
                }
            }
            return frames;


        }

        public void Pintar(Graphics g)
        {
            g.DrawImage(
                Skin.Image,
                Skin.Left,
                Skin.Top,
                Skin.Width,
                Skin.Height
            );
        }

        public override void Animar(string prefijo, int numFrames)
        {

        }
    }
}
