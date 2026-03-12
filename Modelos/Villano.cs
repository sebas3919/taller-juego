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
    internal class Villano : Personaje
    {
        public Animator animator;
        public enum EstadosVillano
        {
            Idle,
            CaminandoDerecha,
            AtacandoDerecha,
            CorriendoIzquierda

        }
        int Frame = 0;
        public EstadosVillano EstadoActual = EstadosVillano.Idle;
        public Villano()
        {
            Skin = new PictureBox();
            string rutaImagen = Path.Combine(Application.StartupPath, "Animaciones", "Villano", "Left - Idle", "Left - Idle_000.png");
            Skin.Image = Image.FromFile(rutaImagen);
            
            Skin.Width = 100;
            Skin.Height = 100;
            Skin.Location = new Point(500, 150);

            animator = new Animator();
            animator.personaje = this;

            var idle = new Animacion()
            {
                Name = "Left - Idle",
                frames = LeerImagenes("Left - Idle"),
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

            var runnigLeft = new Animacion()
            {
                Name = "Left - Running",
                frames = LeerImagenes("Left - Running"),
                framerate = 1
            };

            animator.AddAnimation(idle);
            animator.AddAnimation(walkLeft);
            animator.AddAnimation(walkRight);
            animator.AddAnimation(attackRight);
            animator.AddAnimation(runnigLeft);

            animator.Play("Left - Idle");
        }

        public void ActualizarPantalla()
        {
            animator.Update();
            switch (EstadoActual)
            {
                case EstadosVillano.CaminandoDerecha:
                    animator.Play("Right - Walking");
                    break;
                case EstadosVillano.Idle:
                    animator.Play("Left - Idle");
                    break;
                case EstadosVillano.AtacandoDerecha:
                    animator.Play("Right - Attacking");
                    break;
                case EstadosVillano.CorriendoIzquierda:
                    animator.Play("Left - Running");
                    break;
            }
        }

        public override void Animar(string prefijo, int numFrames)
        {

            

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
               "Villano",
               prefijo);

            if(!Directory.Exists(rutaImagen))
            {
                Console.WriteLine($"La ruta {rutaImagen} no existe.");
                return frames;
            }
            foreach (var file in Directory.GetFiles(rutaImagen))
            {
                using (var imgTemp = Image.FromFile(file))
                {
                    frames.Add(new Bitmap(imgTemp)); // 🔥 clona y libera el archivo
                }
            }
            return frames;


        }
    }
}
