using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Panel[] Ps = {c1,  c2,  c3,  c4,  c5,  c6,  c7,  c8,  c9,  c10,
                          c11, c12, c13, c14, c15, c16, c17, c18, c19, c20};
            for (int i = 0; i < Ps.Length; i++)
                Ps[i].Click += mudaCor;
            expessuras.BringToFront();
        }

        string modo = "linha";
        int expessura = 0;        

        // -------------- Principal (DESENHOS) --------------
        private void panel1_Paint(object sender, PaintEventArgs e) {
            desenhar(e);
        }

        // -------------- CLIQUE NO QUADRO --------------
        private void panel1_MouseUp(object sender, MouseEventArgs e) {
            quadro.Items.Add(e.X);
            quadro.Items.Add(e.Y);
            if (quadro.Items.Count % 7 != 2) {
                quadro.Items.Add(expessura);
                quadro.Items.Add(modo);
                quadro.Items.Add(cor.BackColor.R+";"+cor.BackColor.G+";"+cor.BackColor.B);
                this.Refresh();
            }
        }

        // -------------- DEFINIR FORMAS --------------
        string[] formas = { "linha", "quadrado", "losango", "triangulo", "pentagono" };

        private void button1_Click(object sender, EventArgs e) {
            selecionarForma(0);
        }

        private void button2_Click(object sender, EventArgs e) {
            selecionarForma(1);
        }

        private void button3_Click(object sender, EventArgs e) {
            selecionarForma(2);
        }

        private void button4_Click(object sender, EventArgs e) {
            selecionarForma(3);
        }

        private void button5_Click(object sender, EventArgs e) {
            selecionarForma(4);
        }

        // -------------- DEFINIR EXPESSURAS --------------
        // Menu
        private void menu_Click(object sender, EventArgs e) {
            expessuras.Visible = !expessuras.Visible;
        }

        private void ex1_Click(object sender, EventArgs e) {
            selecionarExp(0);
        }

        private void ex2_Click(object sender, EventArgs e) {
            selecionarExp(1);
        }

        private void ex3_Click(object sender, EventArgs e) {
            selecionarExp(3);
        }

        private void ex4_Click(object sender, EventArgs e) {
            selecionarExp(5);
        }

        // -------------- FUNÇÕES --------------
        private void mudaCor(object sender, EventArgs e) {
            cor.BackColor = ((Panel) sender).BackColor;
        }

        private void retaDDA(PaintEventArgs e, int x0, int y0, int x1, int y1, Color c) {
            int dx = x1 - x0;
            int dy = y1 - y0;
            double x = x0;
            double y = y0;
            double s = 0;
            s = (Math.Abs(dx) > Math.Abs(dy)) ? Math.Abs(dx) : Math.Abs(dy);
            s = (s == 0) ? 1 : s;
            double xi = dx / s;
            double yi = dy / s;
            pintap(e, (int)x, (int)y, c);
            for (int i = 0; i < Math.Abs(s); i++) {
                x += xi;
                y += yi;
                pintap(e, (int)x, (int)y, c);
            }
        }

        private void pintap(PaintEventArgs e, int x, int y, Color c) {
            //Pen pen = new Pen(c, expessura);
            //e.Graphics.DrawLine(pen, x, y, x + 1, y);
            e.Graphics.FillRectangle(new SolidBrush(c), x, y, 1 + expessura, 1 + expessura);
        }

        private void selecionarExp(int num) {
            expessura = num;
            ex1.Enabled = num != 0;
            ex2.Enabled = num != 1;
            ex3.Enabled = num != 3;
            ex4.Enabled = num != 5;
            expessuras.Visible = false;
        }

        private void selecionarForma(int forma) {
            modo = formas[forma];
            button1.Enabled = forma != 0;
            button2.Enabled = forma != 1;
            button3.Enabled = forma != 2;
            button4.Enabled = forma != 3;
            button5.Enabled = forma != 4;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.B) {
                quadro.Items.Clear();
                this.Refresh();
            }
            else if (e.KeyCode == Keys.Z && e.Control) {
                for (int i = 0; i<7; i++)
                    quadro.Items.RemoveAt(quadro.Items.Count-1);
                this.Refresh();
            }
        }

        // -------------- PINTAR LISTA --------------

        private void desenhar(PaintEventArgs e) {
            int backupExpessura = expessura;
            string backupForma = modo;
            Color backupCor = cor.BackColor;
            for (int qdr = 0; qdr < quadro.Items.Count; qdr += 7) {
                if (quadro.Items.Count - qdr < 7)
                    break;
                int P1x = int.Parse(quadro.Items[qdr].ToString());
                int P1y = int.Parse(quadro.Items[qdr + 1].ToString());
                int P2x = int.Parse(quadro.Items[qdr + 2].ToString());
                int P2y = int.Parse(quadro.Items[qdr + 3].ToString());
                expessura = int.Parse(quadro.Items[qdr + 4].ToString());
                modo = quadro.Items[qdr + 5].ToString();
                string[] rgb = quadro.Items[qdr + 6].ToString().Split(';');
                cor.BackColor = Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));

                int meioHorz = (P1x + P2x) / 2;
                int meioVert = (P1y + P2y) / 2;
                switch (modo) {
                    case "linha":
                        retaDDA(e, P1x, P1y, P2x, P2y, cor.BackColor);
                        break;
                    case "quadrado":
                        retaDDA(e, P1x, P1y, P2x, P1y, cor.BackColor);
                        retaDDA(e, P1x, P1y, P1x, P2y, cor.BackColor);
                        retaDDA(e, P2x, P1y, P2x, P2y, cor.BackColor);
                        retaDDA(e, P1x, P2y, P2x, P2y, cor.BackColor);
                        break;
                    case "losango":
                        retaDDA(e, P1x, meioVert, meioHorz, P1y, cor.BackColor);
                        retaDDA(e, P2x, meioVert, meioHorz, P1y, cor.BackColor);
                        retaDDA(e, P1x, meioVert, meioHorz, P2y, cor.BackColor);
                        retaDDA(e, P2x, meioVert, meioHorz, P2y, cor.BackColor);
                        break;
                    case "triangulo":
                        retaDDA(e, P1x, P2y, meioHorz, P1y, cor.BackColor);
                        retaDDA(e, P2x, P2y, meioHorz, P1y, cor.BackColor);
                        retaDDA(e, P1x, P2y, P2x, P2y, cor.BackColor);
                        break;
                    case "pentagono":
                        int[] A = {P1x+(int)(3*(P2x - P1x)/13), P2y},
                              B = {P2x-(int)(3*(P2x - P1x)/13), P2y},
                              C = {P2x, P1y+(int)(2*(P2y - P1y)/5)},
                              D = {(int)((P2x + P1x)/2), P1y},
                              E = {P1x, P1y+(int)(2*(P2y - P1y)/5)};
                        retaDDA(e, A[0], A[1], B[0], B[1], cor.BackColor);
                        retaDDA(e, B[0], B[1], C[0], C[1], cor.BackColor);
                        retaDDA(e, C[0], C[1], D[0], D[1], cor.BackColor);
                        retaDDA(e, D[0], D[1], E[0], E[1], cor.BackColor);
                        retaDDA(e, E[0], E[1], A[0], A[1], cor.BackColor);
                        
                        break;
                    default:
                        break;
                }
            }
            modo = backupForma;
            cor.BackColor = backupCor;
            expessura = backupExpessura;
        }
        private void cor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            cor.BackColor = colorDialog1.Color;
        }
    }
}