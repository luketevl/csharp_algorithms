using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Trabalho trab = new Trabalho();
             
            // Chamando metodo que le o arquivo
            trab.lerArquivo(@"D:\trabalhografos\caminhos.txt");

            Console.WriteLine("Aguarde estamos montando a estrutura");

            // Começo da estrutura
            trab.inicioSolucao();

            // Pergunta Qual algortimo deseja usar
            Console.WriteLine("Digite a opção Desejada" );
            Console.WriteLine("1 - Heuristica Propria" );
            Console.WriteLine("2 - Algortimo Guloso");
            
            // Captura da opção digitada
            string opcao = Console.ReadLine();
            if (opcao.Equals("1"))
            // Instanciando classe
            {
                trab.heuristicaAleatoria();
            }
            else if (opcao.Equals("2"))
            {
                trab.algoritmoGuloso();
            }
            
        }
    }

     // Classe responsável por realizar a leitura e impressão dos Vertices e Pesos 
     
    class Trabalho 
    {
        
        // Atributos
        private Double menorDistanciaTotal      = 0;
        private Double DistanciaTotal = 0;
        private string[] conteudoArquivo;
        private string[] conteudoArquivoLido;
        private ArrayList listaCaminhos         = new ArrayList();
      
        // Metodo responsavel por ler o arquivo
        public void lerArquivo(string caminhoArquivo)
        {
            // Utilizado metodo do C# que le arquivos e cria um array com as linhas
            this.conteudoArquivoLido = System.IO.File.ReadAllLines(caminhoArquivo);
            this.removeCabecalhoArquivo();
            
        }
        // Remove cabecalho do aquivo
        public void removeCabecalhoArquivo() 
        { 
            // Ficara apenas O NOVO CONTEUDO
            ArrayList novoConteudo = new ArrayList();
           
            // Controle do Array
            int numeroLinha = 0;

            // Variavel que faz controle de quando comeca as coordenadas
            Boolean inicioCoordenada = false;

            // Loop ue remove o cabecalho
            for (int i = 0; i < this.conteudoArquivoLido.Length; i++) 
            {
                // Verifica se a linha é para começar as coordenadas
                if (this.conteudoArquivoLido[i].Contains("COORD_SECTION"))
                {
                    // Seta que ja pode adicionar as coordenadas
                    inicioCoordenada = true;

                }
                // Verifica se é FIM DO ARQUIVO se for sai d loop
                else if (this.conteudoArquivoLido[i].Contains("EOF"))
                {
                    inicioCoordenada = false;
                    break;
                }
                // Caso as coordenadas possam ser gravadas
                if (inicioCoordenada)
                {

                    // Verifica se incremento do indice NAO vai ultrapassar o LIMITE do array
                    if ((i + 1) != this.conteudoArquivoLido.Length)
                    {
                        // VERIFICA SE NAO EH FIM DA LINHA
                        if (!this.conteudoArquivoLido[(i+1)].Contains("EOF"))
                        {
                            // Adiciona linha dos VERTICES
                            novoConteudo.Add(this.conteudoArquivoLido[(i + 1)]);
                        }
                        
                    }
                    
                }
            }
            // Seta array com tamanho dos indices
            this.conteudoArquivo = new string[novoConteudo.Count];

            // Faz com que conteudo seja APENAS os vertices
            foreach (var conteudo in novoConteudo)
            {
                this.conteudoArquivo[numeroLinha] = (string)conteudo;
                numeroLinha++;
            }
        }

        // Heuristica Aleatoria
        public void heuristicaAleatoria()
        {

            // Heuristica
            // Gera um numero aleatorio que esteja entre a quantidade de vertices
            // Gera um novo numero aleatoria que esteja entre a quantidad de vertices para que seja o vertice VISITADO
            // Enquanto nao passar por todos os vertices nao acaba

            // Gerando numero aleatorio
            Random random = new Random();
            int numeroAletorio = random.Next(this.conteudoArquivo.Length);

            // Array para saber quais foram visitados
            ArrayList visitados = new ArrayList();

            // Adiciona o vertice escolhido aleatorio na lista de visitados
            visitados.Add(numeroAletorio);

            // Controlar quem é o vertice do caminho
            int numeroDoVertice = numeroAletorio;

            // Enquanto a lista de visitados não tiver o mesmo tamanho do numero de vertices continua
            while (visitados.Count != this.conteudoArquivo.Length)
            {

                // Contem a lista de vizinhos
                double[] listaVizinhos = (double[])listaCaminhos[numeroDoVertice];

                // Gera números aleatorio para OS VERTICES ENQUANTO esse numero aleatoria nao tenha sido visitado ainda
                while (jaFoiVisitado(visitados, numeroDoVertice)) 
                {
                    // Gera o proximo VERTICE a ser visitado ALEATORIAMENTE
                    numeroDoVertice = random.Next(this.conteudoArquivo.Length);
                }
                 
                // Recebe o custo
                double valor = listaVizinhos[numeroDoVertice];
            
                //  Vai somando distancia dos vertices visitados ALEATORIOS
                DistanciaTotal += valor;

                // Verfica se é o vertice INICIAL e imprime ele
                if (visitados.Count == 1)
                {
                    // IMPRIMIR VERTICE incial e peso para o PROXIMO
                    imprimirVerticeDistancia(numeroAletorio, valor);
                }
                else
                {
                    // IMPRIMIR VERTICE de MENOR CUSTO e o custo para o proximo
                    imprimirVerticeDistancia(numeroDoVertice, valor);

                }

                //  Adiciona vertice do menor caminho a lista de adicionados
                visitados.Add(numeroDoVertice);
            }
            // IMPRIME DISTANCIA TOTAL
            imprimirDistanciaTotal(this.DistanciaTotal);
        }


        // Algortimo GULOSO
        public void algoritmoGuloso() 
        {

            // Heuristica
            // Gera um numero aleatorio que esteja entre a quantidade de vertices
            // Busca os vizinhos e ve qual tem o menor peso
            // Enquanto nao passar por todos os vertices nao acaba

            // Gerando numero aleatorio
            Random random = new Random();
            int numeroAletorio = random.Next(this.conteudoArquivo.Length);

            // Array para saber quais foram visitados
            ArrayList visitados = new ArrayList();

            // Adiciona o vertice escolhido aleatorio na lista de visitados
            visitados.Add(numeroAletorio);

            // Controlar quem é o vertice de menor caminho
            int numeroDoVerticeMenorCaminho = numeroAletorio;

            // Enquanto a lista de visitados não tiver o mesmo tamanho do numero de vertices continua
            while (visitados.Count != this.conteudoArquivo.Length)
            {

                // Contem a lista de vizinhos
                double[] listaVizinhos = (double[])listaCaminhos[numeroDoVerticeMenorCaminho];

                // Controla quem tem menor distancia
                double valor = Double.MaxValue;


                // Percorre TODOS OS VIZINHOS do VERTICE ATUAL (Menor Custo) 
                for (int j = 0; j < listaCaminhos.Count; j++)
                {
                    // Verifica se o custo é realmente o menor se não é zero(ele proprio) e se ainda NÃO foi visitado
                    if (listaVizinhos[j] < valor && listaVizinhos[j] != 0 && !jaFoiVisitado(visitados, j))
                    {
                        // Recebe o menor custo
                        valor = listaVizinhos[j];

                        // Vertice do MENOR custo
                        numeroDoVerticeMenorCaminho = j;
                    }

                }
                //  Vai somando distancia de menor custp TOTAL
                menorDistanciaTotal += valor;

                // Verfica se é o vertice INICIAL e imprime ele
                if (visitados.Count == 1)
                {
                    // IMPRIMIR VERTICE incial e peso para o PROXIMO
                    imprimirVerticeDistancia(numeroAletorio, valor);
                }
                else
                {
                    // IMPRIMIR VERTICE de MENOR CUSTO e o custo para o proximo
                    imprimirVerticeDistancia(numeroDoVerticeMenorCaminho, valor);

                }

                //  Adiciona vertice do menor caminho a lista de adicionados
                visitados.Add(numeroDoVerticeMenorCaminho);
            }
            // IMPRIME DISTANCIA TOTAL
            imprimirDistanciaTotal(this.menorDistanciaTotal);
        }

        // Metodo que resolve o problema
        public void inicioSolucao()
        {
           
            // Calcular a distancia entre todos os pontos
            for (int i = 0; i < this.conteudoArquivo.Length; i++) 
            {

                // Quebrando linha em posicoes do array
                string[] verticeDados = this.conteudoArquivo[i].Split(' ');


                // Criando a variavel matriz em que cada posicao visita todos os vertices e calcula a distancia entre eles
                double[] matriz = new double[this.conteudoArquivo.Length];

                // Contador dos vertices vistados
                int k = 0;

                // Loop para percorrer todos os vertices
                for (int j = 0; j < this.conteudoArquivo.Length; j++)
                {
                    // Quebrando linha em posicoes do array
                    string[] verticeDadosPercorridos = this.conteudoArquivo[j].Split(' ');

                    // Calculando distancia
                    double distancia = this.calculaDistancia(verticeDados[1], verticeDadosPercorridos[1], verticeDados[2], verticeDadosPercorridos[2]);

                    // Adicionando no vertice percorrido qual a sua distancia.
                    matriz[k] = distancia;
                    // Incrementando contador dos vertices percorridos.
                    k++;

                }
                // Adicionando no vertice, qual são os vertices visitados e as distancias entre eles.
                listaCaminhos.Add(matriz);
              
            }

        }
        
        
        // Metodo responsavel por verificar se elemento já foi visitado
        private Boolean jaFoiVisitado(ArrayList visitados, int vertice) 
        {
            // Pecorre vertice por vertice JA VISITADOS
            foreach (var visitado in visitados)
            {
                // Converte numero do vertice para INT
                int verticeConvertido = (int)visitado;
                
                // Verifica se vertice ja foi VISITADO
                if(verticeConvertido == vertice)
                {
                    return true;
                }
            }
            // Caso não seja visitado
            return false;
        }
        
        
        
        // Metodo responsavel por imprimir o vertice e peso
        private void imprimirVerticeDistancia(int vertice, double distancia)
        {
            Console.WriteLine((vertice+1) + " " +  distancia);
        }

        // Metodo responsavel por imprimir a distancia total
        private void imprimirDistanciaTotal(Double distanciaTotal)
        {
            Console.WriteLine(distanciaTotal);
            Console.ReadKey();
        }

        // Metodo que calcula a distancia entre dois Pontos
        private double calculaDistancia(string x1, string x2, string y1, string y2)
        {
            // Removendo ponto da string
            x1 = x1.Replace(".", "" );
            // Convertendo string para double
            double x1Double = Convert.ToDouble(x1);

            x2 = x2.Replace(".", "");
            double x2Double = Convert.ToDouble(x2);


            y1 = y1.Replace(".", "");
            double y1Double = Convert.ToDouble(y1);

            y2 = y2.Replace(".", "");
            double y2Double = Convert.ToDouble(y2);

            // Variavel que vai receber o valor da distancia
            double distancia;

            // Calculo da distancia
                // Aplica teorema de pitagoras, substraindo as coordenadas dos vertices recebidos no parametro
            distancia = Math.Sqrt(Math.Pow(x1Double - x2Double, 2) + Math.Pow(y1Double - y2Double , 2));
            return Math.Abs(distancia);
        }
    }

}