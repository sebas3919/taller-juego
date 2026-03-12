using Animaciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Animaciones.Componentes
{
    internal class Animator
    {
        private Dictionary<string, Animacion> animations = new Dictionary<string, Animacion>();

        private Animacion currentAnimation = new Animacion();
        int frameIndex = 0;
        int counter = 0;
        public Personaje personaje = null;
        public Animator() { }
        public Animator(Animacion animacion)
        {

            animations[animacion.Name] = animacion;
        }

        public void Play(string name)
        {
            if (!animations.ContainsKey(name)) return;
            if (currentAnimation != null && currentAnimation.Name == name) return;

            currentAnimation = animations[name];
            Console.WriteLine($"Reproduciendo animación: {currentAnimation.Name}");
            frameIndex = 0;
            counter = 0;
        }
        public void AddAnimation(Animacion anim)
        {
            animations[anim.Name] = anim;
        }
        public void Update()
        {

            if (personaje == null) return;
            if (currentAnimation == null || currentAnimation.frames == null || currentAnimation.frames.Count == 0) return;
            Console.WriteLine("Animaciones" + animations.Count + " "+ currentAnimation.framerate);
            counter++;
            if (counter >= currentAnimation.framerate)
            {
                
                personaje.Skin.Image = currentAnimation.frames[frameIndex];
                frameIndex++;
                if (frameIndex >= currentAnimation.frames.Count)
                {
                    frameIndex = 0;
                }
                counter = 0;
            }
            

        }


    }
}
