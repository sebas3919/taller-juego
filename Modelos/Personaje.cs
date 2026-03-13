using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animaciones.Modelos
{
    internal abstract class Personaje
    {
        
        public string Nombre { get; set; }
        public int Energia { get; set; }
        public PictureBox Skin { get; set; }

        public int EnergiaMaxima { get; set; }

        public int Danio { get; set; }

        public void RecibirDanio(int cantidad)
        {
            Energia -= cantidad;

            if (Energia < 0)
                Energia = 0;
        }

        public bool EstaVivo()
        {
            return Energia > 0;
        }
        public void Mover(int orientacion) {
           if(orientacion > 0) Skin.Left += 1;
           if (orientacion <= 0) Skin.Left -= 1;
        }

        public abstract void Animar(string prefijo, int numFrames);
    }
}
