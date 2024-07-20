namespace Snake
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer temporizadorJogo = new System.Windows.Forms.Timer();
        private List<Point> cobra = new List<Point>();
        private Point comida = new Point();
        private Direcao direcao = Direcao.Direita;
        private int pontuacao = 0;
        private Random aleatorio = new Random();
        private const int tamanhoGrade = 20; // Tamanho da grade
        private const int tamanhoCelula = 20; // Tamanho de cada célula

        public Form1()
        {
            InitializeComponent();

            ConfigurarLayoutFormulario();

            // Inicializa o jogo
            InicializarJogo();

            // Configura o temporizador do jogo
            temporizadorJogo.Interval = 200; // Velocidade do jogo
            temporizadorJogo.Tick += AtualizarTela;
            temporizadorJogo.Start();

            // Captura as teclas pressionadas
            this.KeyDown += new KeyEventHandler(OnTeclaPressionada);
        }

        private void ConfigurarLayoutFormulario()
        {
            this.ClientSize = new Size(tamanhoGrade * tamanhoCelula, tamanhoGrade * tamanhoCelula);
            this.Text = "Jogo da Cobrinha";
            this.DoubleBuffered = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Gray;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        // Inicializa o estado do jogo
        private void InicializarJogo()
        {
            cobra.Clear();
            cobra.Add(new Point(tamanhoGrade / 2, tamanhoGrade / 2)); // Posição inicial da cobra
            direcao = Direcao.Direita;
            pontuacao = 0;
            GerarComida();
        }

        // Gera a comida em uma posição aleatória
        private void GerarComida()
        {
            comida = new Point(aleatorio.Next(0, tamanhoGrade), aleatorio.Next(0, tamanhoGrade));
        }

        // Atualiza a tela do jogo
        private void AtualizarTela(object sender, EventArgs e)
        {
            MoverCobra();
            VerificarColisao();
            this.Invalidate(); // Redesenha a tela
        }

        // Move a cobra na direção atual
        private void MoverCobra()
        {
            // Move o corpo da cobra
            for (int i = cobra.Count - 1; i > 0; i--)
            {
                cobra[i] = cobra[i - 1];
            }

            // Move a cabeça da cobra
            switch (direcao)
            {
                case Direcao.Direita:
                    cobra[0] = new Point(cobra[0].X + 1, cobra[0].Y); // Direita
                    break;
                case Direcao.Baixo:
                    cobra[0] = new Point(cobra[0].X, cobra[0].Y + 1); // Baixo
                    break;
                case Direcao.Esquerda:
                    cobra[0] = new Point(cobra[0].X - 1, cobra[0].Y); // Esquerda
                    break;
                case Direcao.Cima:
                    cobra[0] = new Point(cobra[0].X, cobra[0].Y - 1); // Cima
                    break;
            }
        }

        // Verifica as colisões
        private void VerificarColisao()
        {
            // Colisão com os limites da grade
            if (cobra[0].X < 0 || cobra[0].Y < 0 || cobra[0].X >= tamanhoGrade || cobra[0].Y >= tamanhoGrade)
            {
                GameOver();
            }

            // Colisão com o próprio corpo
            for (int i = 1; i < cobra.Count; i++)
            {
                if (cobra[0] == cobra[i])
                {
                    GameOver();
                }
            }

            // Colisão com a comida
            if (cobra[0] == comida)
            {
                cobra.Add(comida); // Aumenta o tamanho da cobra
                pontuacao++; // Incrementa a pontuação
                GerarComida(); // Gera nova comida
            }
        }

        // Encerrar o jogo
        private void GameOver()
        {
            temporizadorJogo.Stop();
            MessageBox.Show("Fim de Jogo! Pontuação: " + pontuacao, "Jogo da Cobrinha");
            InicializarJogo();
            temporizadorJogo.Start();
        }

        // Captura a tecla pressionada e muda a direção da cobra
        private void OnTeclaPressionada(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (direcao != Direcao.Baixo) direcao = Direcao.Cima; // Cima
                    break;
                case Keys.Down:
                    if (direcao != Direcao.Cima) direcao = Direcao.Baixo; // Baixo
                    break;
                case Keys.Left:
                    if (direcao != Direcao.Direita) direcao = Direcao.Esquerda; // Esquerda
                    break;
                case Keys.Right:
                    if (direcao != Direcao.Esquerda) direcao = Direcao.Direita; // Direita
                    break;
            }
        }

        // Desenha a cobra, a comida e a pontuação na tela
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            // Desenha a cobra
            foreach (Point p in cobra)
            {
                canvas.FillRectangle(Brushes.Blue, new Rectangle(p.X * tamanhoCelula, p.Y * tamanhoCelula, tamanhoCelula, tamanhoCelula));
            }

            // Desenha a comida
            canvas.FillRectangle(Brushes.Green, new Rectangle(comida.X * tamanhoCelula, comida.Y * tamanhoCelula, tamanhoCelula, tamanhoCelula));

            // Desenha a pontuação
            canvas.DrawString("Pontuação: " + pontuacao, this.Font, Brushes.Black, new PointF(10, 10));
        }

        private enum Direcao
        {
            Direita,
            Baixo,
            Esquerda,
            Cima
        }
    }
}
