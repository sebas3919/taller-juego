using Animaciones.Componentes;
using Animaciones.Modelos;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animaciones
{
    public partial class Form1 : Form
    {
        // GRID
        int filas = 8;
        int columnas = 12;
        int tamanoCelda = 80;

        int[,] mapa;

        // POSICION EN GRID
        int heroeFila = 2;
        int heroeColumna = 1;

        int villanoFila = 2;
        int villanoColumna = 8;

        int asesinoFila = 5;
        int asesinoColumna = 10;

        enum Turno
        {
            Heroe,
            Asesino,
            Villano,       
            Animando
        }

        Turno turnoActual = Turno.Heroe;
        

        Personaje heroe = new Heroe();
        Personaje villano = new Villano();
        Personaje asesino = new Asesino();

        Timer loop = new Timer();

        public Form1()
        {
            InitializeComponent();
            heroe.Energia = 100;
            int espacioBarras = 120;
            this.Width = columnas * tamanoCelda + 20;
            this.Height = filas * tamanoCelda + espacioBarras;
            heroe.Energia = heroe.EnergiaMaxima;
            villano.Energia = villano.EnergiaMaxima;

            progressBar1.Maximum = heroe.EnergiaMaxima;
            progressBar2.Maximum = villano.EnergiaMaxima;

            progressBar1.Value = heroe.Energia;
            progressBar2.Value = villano.Energia;

            progressBar1.Location = new Point(50, filas * tamanoCelda + 20);
            progressBar1.Width = 250;

            progressBar2.Location = new Point(350, filas * tamanoCelda + 20);
            progressBar2.Width = 250;

            villano.Energia = 100;
            heroe.Energia = 100;

            progressBar1.Maximum = 100;
            progressBar1.Value = heroe.Energia;

            progressBar2.Maximum = 100;
            progressBar2.Value = villano.Energia;

            this.KeyPreview = true;
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.DoubleBuffered = true;

            mapa = new int[filas, columnas];

            // EJEMPLO DE OBSTACULOS
            mapa[3, 5] = 1;
            mapa[4, 5] = 1;
            mapa[5, 5] = 1;

            // POSICION INICIAL
            ActualizarPosicionHeroe();
            ActualizarPosicionVillano();
            ActualizarPosicionAsesino();

            KeyDown += OnKeyPresDown;
            KeyUp += OnKeyUp;
            Paint += OnPaint;

            
            loop.Interval = 16;
            loop.Tick += OnPlay;
            loop.Start();
        }

        void ActualizarPosicionHeroe()
        {
            heroe.Skin.Location = new Point(
                heroeColumna * tamanoCelda,
                heroeFila * tamanoCelda
            );
        }

        void ActualizarPosicionVillano()
        {
            villano.Skin.Location = new Point(
                villanoColumna * tamanoCelda,
                villanoFila * tamanoCelda
            );
        }

        void ActualizarPosicionAsesino()
        {
            asesino.Skin.Location = new Point(
                asesinoColumna * tamanoCelda,
                asesinoFila * tamanoCelda
            );
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // DIBUJAR GRID
            for (int f = 0; f < filas; f++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    Rectangle celda = new Rectangle(
                        c * tamanoCelda,
                        f * tamanoCelda,
                        tamanoCelda,
                        tamanoCelda
                    );

                    g.DrawRectangle(Pens.Black, celda);

                    // obstaculos
                    if (mapa[f, c] == 1)
                    {
                        g.FillRectangle(Brushes.DarkGray, celda);
                    }
                }
            }

            // HEROE
            g.DrawImage(heroe.Skin.Image, new Rectangle(
                heroe.Skin.Location.X,
                heroe.Skin.Location.Y,
                heroe.Skin.Width,
                heroe.Skin.Height));

            // VILLANO
            g.DrawImage(villano.Skin.Image, new Rectangle(
                villano.Skin.Location.X,
                villano.Skin.Location.Y,
                villano.Skin.Width,
                villano.Skin.Height));

            g.DrawImage(asesino.Skin.Image, new Rectangle(
            asesino.Skin.Location.X,
            asesino.Skin.Location.Y,
            asesino.Skin.Width,
            asesino.Skin.Height));

            // TEXTO TURNO
            g.DrawString(
                $"Turno: {turnoActual}",
                new Font("Arial", 16),
                Brushes.Black,
                20,
                20
            );

            

           
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            ((Heroe)heroe).EstadoActual = Heroe.EstadosHeroe.Idle;
            ((Asesino)asesino).EstadoActual = Asesino.EstadosAsesino.Idle;
        }

        private void OnKeyPresDown(object sender, KeyEventArgs e)
        {
            /*if (turnoActual != Turno.Heroe)
                return;*/

            if (turnoActual == Turno.Heroe)
            {
                ControlHeroe(e);
            }

            if (turnoActual == Turno.Asesino)
            {
                ControlAsesino(e);
            }

        
        }

        void ControlHeroe(KeyEventArgs e)
        {
            int nuevaFila = heroeFila;
            int nuevaColumna = heroeColumna;
            if (e.KeyCode == Keys.Right)
            {
                nuevaColumna++;
                ((Heroe)heroe).EstadoActual = Heroe.EstadosHeroe.CaminandoDerecha;
            }
            if (e.KeyCode == Keys.Left)
            {
                nuevaColumna--;
                ((Heroe)heroe).EstadoActual = Heroe.EstadosHeroe.CaminandoIzquierda;
            }
            if (e.KeyCode == Keys.Up)
                nuevaFila--;
            if (e.KeyCode == Keys.Down)
                nuevaFila++;
            if (PuedeMover(nuevaFila, nuevaColumna))
            {
                heroeFila = nuevaFila;
                heroeColumna = nuevaColumna;
                ActualizarPosicionHeroe();
            }
            if (e.KeyCode == Keys.Space)
            {
                ((Heroe)heroe).EstadoActual = Heroe.EstadosHeroe.AtacandoDerecha;
                turnoActual = Turno.Asesino;
            }
        }

        void ControlAsesino(KeyEventArgs e)
        {
            int nuevaFila = asesinoFila;
            int nuevaColumna = asesinoColumna;

            if (e.KeyCode == Keys.D)
            {
                nuevaColumna++;
                ((Asesino)asesino).EstadoActual = Asesino.EstadosAsesino.CaminandoDerecha;
            }

            if (e.KeyCode == Keys.A)
            {
                nuevaColumna--;
                ((Asesino)asesino).EstadoActual = Asesino.EstadosAsesino.CaminandoIzquierda;
            }

            if (e.KeyCode == Keys.W)
                nuevaFila--;

            if (e.KeyCode == Keys.S)
                nuevaFila++;

            if (PuedeMover(nuevaFila, nuevaColumna))
            {
                asesinoFila = nuevaFila;
                asesinoColumna = nuevaColumna;
                ActualizarPosicionAsesino();
            }

            if (e.KeyCode == Keys.X)
            {
                ((Asesino)asesino).EstadoActual = Asesino.EstadosAsesino.AtacandoDerecha;
                turnoActual = Turno.Villano;
            }
        }
        bool PuedeMover(int fila, int columna)
        {
            if (fila < 0 || fila >= filas)
                return false;

            if (columna < 0 || columna >= columnas)
                return false;

            if (mapa[fila, columna] == 1)
                return false;

            return true;
        }
        void ActualizarBarrasEnergia()
        {
            progressBar1.Value = heroe.Energia;
            progressBar2.Value = villano.Energia;
        }
        private int contadorTurno = 0;

        private void OnPlay(object sender, EventArgs e)
        {
            ((Heroe)heroe).ActualizarPantalla();
            ((Villano)villano).ActualizarPantalla();
            ((Asesino)asesino).ActualizarPantalla();

            //((Heroe)heroe).animator.Update();
            ((Villano)villano).animator.Update();

            ((Heroe)heroe).animator.Update();
            ((Asesino)asesino).animator.Update();

            if (turnoActual == Turno.Villano)
            {
                TurnoVillano();
            }

            ActualizarBarrasEnergia(); 

            Invalidate();
        }

        void TurnoVillano()
        {
            contadorTurno++;

            if (contadorTurno == 30)
            {
                ((Villano)villano).EstadoActual = Villano.EstadosVillano.AtacandoDerecha;
            }

            if (contadorTurno > 120)
            {
                contadorTurno = 0;

                ((Villano)villano).EstadoActual = Villano.EstadosVillano.Idle;

                turnoActual = Turno.Heroe;
            }
        }
    }
}