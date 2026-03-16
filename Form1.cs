using Animaciones.Componentes;
using Animaciones.Modelos;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Animaciones
{
    public partial class Form1 : Form
    {
        int filas = 8;
        int columnas = 12;
        int tamanoCelda = 80;

        int[,] mapa;

        int heroeFila = 2;
        int heroeColumna = 1;

        int villanoFila = 2;
        int villanoColumna = 8;

        bool juegoTerminado = false;
        int buffsActivos = 0;
        Image imgVida = Properties.Resources.Hp;
        Image imgFuerza = Properties.Resources.Damage;
        

        enum Turno
        {
            Heroe,
            Villano
        }

        Turno turnoActual = Turno.Heroe;

        Personaje heroe = new Heroe();
        Personaje villano = new Villano();

        Timer loop = new Timer();
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            heroe.EnergiaMaxima = 400;
            villano.EnergiaMaxima = 1000;

            heroe.Energia = 400;
            villano.Energia = 1000;

            int espacioBarras = 120;

            this.Width = columnas * tamanoCelda + 20;
            this.Height = filas * tamanoCelda + espacioBarras;

            heroe.Energia = 400;
            villano.Energia = 1000;

            heroe.Danio = 40;
            villano.Danio = 80;

            progressBar1.Maximum = 400;
            progressBar1.Value = heroe.Energia;

            progressBar2.Maximum = 1000;
            progressBar2.Value = villano.Energia;

            progressBar1.Location = new Point(50, filas * tamanoCelda + 20);
            progressBar1.Width = 250;

            progressBar2.Location = new Point(650, filas * tamanoCelda + 20);
            progressBar2.Width = 250;

            this.KeyPreview = true;
            this.DoubleBuffered = true;

            mapa = new int[filas, columnas];

            mapa[3, 5] = 1;
            mapa[4, 5] = 1;
            mapa[5, 5] = 1;

            mapa[1, 3] = 1;
            mapa[2, 3] = 1;

            mapa[6, 7] = 1;
            mapa[6, 8] = 1;

            mapa[2, 9] = 1;
            mapa[3, 9] = 1;

            ActualizarPosicionHeroe();
            ActualizarPosicionVillano();

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

        bool EstanAdyacentes(int f1, int c1, int f2, int c2)
        {
            int df = Math.Abs(f1 - f2);
            int dc = Math.Abs(c1 - c2);

            return (df + dc) == 1;
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (buffsActivos < 3 && rnd.Next(100) < 3)
            {
                int f = rnd.Next(filas);
                int c = rnd.Next(columnas);

                if (mapa[f, c] == 0 && (f != heroeFila || c != heroeColumna) && (f != villanoFila || c != villanoColumna))
                {
                    mapa[f, c] = rnd.Next(2, 4);
                    buffsActivos++; // Aumentamos el contador
                }
            }

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

                    g.FillRectangle(Brushes.LightGreen, celda);
                    g.DrawRectangle(Pens.Black, celda);

                    if (mapa[f, c] == 1)
                    {
                        g.FillRectangle(Brushes.DarkGray, celda);
                    }
                    if (mapa[f, c] == 2)
                    {
                        g.DrawImage(imgVida, c * tamanoCelda + 10, f * tamanoCelda + 10, tamanoCelda - 20, tamanoCelda - 20);
                        g.DrawImage(imgVida, celda);
                    }

                    if (mapa[f, c] == 3)
                    {
                        g.DrawImage(imgFuerza, c * tamanoCelda + 10, f * tamanoCelda + 10, tamanoCelda - 20, tamanoCelda - 20);
                        g.DrawImage(imgFuerza, celda);
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

            // TEXTO TURNO
            g.DrawString(
                $"Turno: {turnoActual}",
                new Font("Arial", 16),
                Brushes.Black,
                20,
                20
            );

            // ===== ESTADISTICAS =====

            string estadoHeroe = heroe.EstaVivo() ? "Vivo" : "Muerto";
            string estadoVillano = villano.EstaVivo() ? "Vivo" : "Muerto";

            Brush colorHeroe = heroe.EstaVivo() ? Brushes.DarkGreen : Brushes.Red;
            Brush colorVillano = villano.EstaVivo() ? Brushes.DarkGreen : Brushes.Red;

            string statsHeroe = $"Heroe | HP: {heroe.Energia}/{heroe.EnergiaMaxima} | {estadoHeroe}";
            string statsVillano = $"Villano | HP: {villano.Energia}/{villano.EnergiaMaxima} | {estadoVillano}";

            // TEXTO HEROE
            g.DrawString(
                statsHeroe,
                new Font("Arial", 12, FontStyle.Bold),
                colorHeroe,
                progressBar1.Location.X,
                progressBar1.Location.Y + 30
            );

            // TEXTO VILLANO
            g.DrawString(
                statsVillano,
                new Font("Arial", 12, FontStyle.Bold),
                colorVillano,
                progressBar2.Location.X,
                progressBar2.Location.Y + 30
            );
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            ((Heroe)heroe).EstadoActual = Heroe.EstadosHeroe.Idle;
        }

        private void OnKeyPresDown(object sender, KeyEventArgs e)
        {
            if (turnoActual != Turno.Heroe)
                return;

            if (!heroe.EstaVivo())
                return;

            ControlHeroe(e);
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

                if (EstanAdyacentes(heroeFila, heroeColumna, villanoFila, villanoColumna))
                {
                    int dano = heroe.Danio;

                    if (heroeColumna < villanoColumna)
                        dano += 10;

                    villano.RecibirDanio(dano);
                }

                turnoActual = Turno.Villano;
            }
            int celda = mapa[heroeFila, heroeColumna];

            if (celda == 2 || celda == 3)
            {
                if (celda == 2)
                {
                    heroe.Energia += 50;
                    if (heroe.Energia > heroe.EnergiaMaxima) heroe.Energia = heroe.EnergiaMaxima;
                }
                else if (celda == 3)
                    heroe.Danio += 10;
            }

            mapa[heroeFila, heroeColumna] = 0;
            buffsActivos--;
            turnoActual = Turno.Villano;
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

        int contadorTurno = 0;

        private void OnPlay(object sender, EventArgs e)
        {
            ((Heroe)heroe).ActualizarPantalla();
            ((Villano)villano).ActualizarPantalla();

            ((Heroe)heroe).animator.Update();
            ((Villano)villano).animator.Update();

            if (turnoActual == Turno.Villano)
                TurnoVillano();

            ActualizarBarrasEnergia();

            if (!juegoTerminado)
            {
                if (!villano.EstaVivo())
                {
                    juegoTerminado = true;
                    loop.Stop();
                    MessageBox.Show("Ganaste!");
                }

                if (!heroe.EstaVivo())
                {
                    juegoTerminado = true;
                    loop.Stop();
                    MessageBox.Show("Perdiste!");
                }
            }

            Invalidate();
        }

        void TurnoVillano()
        {
            contadorTurno++;

            if (contadorTurno == 10)
            {
                if (EstanAdyacentes(villanoFila, villanoColumna, heroeFila, heroeColumna))
                {
                    ((Villano)villano).EstadoActual = Villano.EstadosVillano.AtacandoDerecha;
                    heroe.RecibirDanio(villano.Danio);

                    
                }
                else
                {
                    if (rnd.Next(100) < 40)
                        MovimientoAleatorioVillano();
                    else
                        MoverVillanoIA();
                    
                }
            }

            if (contadorTurno > 40)
            {
                contadorTurno = 0;

                ((Villano)villano).EstadoActual = Villano.EstadosVillano.Idle;

                turnoActual = Turno.Heroe;
            }
        }

        void MovimientoAleatorioVillano()
        {
            int[,] dirs =
            {
                {1,0},
                {-1,0},
                {0,1},
                {0,-1}
            };

            int d = rnd.Next(4);

            int nf = villanoFila + dirs[d, 0];
            int nc = villanoColumna + dirs[d, 1];

            if (PuedeMover(nf, nc))
            {
                villanoFila = nf;
                villanoColumna = nc;
                ActualizarPosicionVillano();
            }
        }

        void MoverVillanoIA()
        {
            int mejorFila = villanoFila;
            int mejorCol = villanoColumna;

            int distActual = Math.Abs(villanoFila - heroeFila) + Math.Abs(villanoColumna - heroeColumna);

            int[,] dirs =
            {
                {1,0},
                {-1,0},
                {0,1},
                {0,-1}
            };

            for (int d = 0; d < 4; d++)
            {
                int nf = villanoFila + dirs[d, 0];
                int nc = villanoColumna + dirs[d, 1];

                if (!PuedeMover(nf, nc))
                    continue;

                int dist = Math.Abs(nf - heroeFila) + Math.Abs(nc - heroeColumna);

                if (dist < distActual)
                {
                    mejorFila = nf;
                    mejorCol = nc;
                    distActual = dist;
                }
            }

            villanoFila = mejorFila;
            villanoColumna = mejorCol;

            ActualizarPosicionVillano();
        }
    }
}