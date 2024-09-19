<h1 align="center"> iHairCare Backend API 🌿 </h1>

<p align="center">
O projeto "iHairCare Backend API" é uma aplicação desenvolvida em C# que oferece uma API RESTful para gerenciar cronogramas de tratamentos capilares. Esta API é responsável por operações de CRUD, como criar, atualizar, listar e excluir cronogramas, conectando-se a um banco de dados MongoDB. O backend foi projetado seguindo uma arquitetura limpa, dividida em camadas, e está sendo desenvolvido utilizando princípios de SOLID e boas práticas de design de software.
</p>

<p align="center">
  <a href="#-tecnologias">Tecnologias</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#-estrutura-do-projeto">Estrutura do Projeto</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
   <a href="#-configurações">Configurações</a>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
  <a href="#-design-da-aplicação">Design da Aplicação</a>
</p>

<br>

## 🚀 Tecnologias

<p>Esse projeto foi desenvolvido com as seguintes tecnologias:</p>

<ul>
  <li>C#</li>
  <li>ASP.NET Core</li>
  <li>MongoDB</li>
  <li>Swagger para documentação da API</li>
  <li>Git e GitHub para controle de versão</li>
</ul>

## 💻 Estrutura do Projeto

<p>A solução é organizada em três camadas principais:</p>

<ul>
  <li><b>API</b>: responsável pelos controladores, configuração do pipeline de middleware e injeção de dependências. Aqui estão as rotas que expõem as operações CRUD da API.</li>
  <li><b>Core</b>: esta camada contém os modelos de domínio, interfaces dos serviços, DTOs e a lógica de negócios. Todos os serviços são organizados aqui, seguindo os princípios de SOLID e desacoplamento.</li>
  <li><b>Infra</b>: implementa a infraestrutura do projeto, como as conexões ao banco de dados (MongoDB), repositórios e implementação das interfaces definidas no Core. Essa camada garante a persistência e recuperação dos dados.</li>
</ul>

<p>Cada camada é desacoplada das outras para facilitar manutenções, testes e futuras expansões.</p>

## ⚙️ Configurações

<ul>
  <li><b>String de Conexão com o MongoDB</b>: A string de conexão está configurada como uma variável de ambiente no Windows, e o acesso a ela é feito através de um provider implementado no código, que é chamado no arquivo Program.cs, garantindo a segurança e flexibilidade no acesso às credenciais do MongoDB para armazenamento dos cronogramas capilares.</li>
  <li><b>Injeção de Dependências</b>: configurada no arquivo <code>Program.cs</code> utilizando o padrão de injeção de dependências para manter o código organizado e fácil de testar.</li>
  <li><b>Swagger</b>: a documentação da API pode ser acessada via <code>/swagger</code>, permitindo a fácil visualização e teste dos endpoints.</li>
</ul>

## 📚 Camadas do Projeto

### **Camada Infra**

<p>Implementa os repositórios que realizam a comunicação com o banco de dados MongoDB. Cada repositório implementa operações CRUD através da interface <code>IScheduleRepository</code>.</p>

<p><b>Factory</b>: Implementada para facilitar a criação de objetos complexos. Seguindo o padrão de <b>Factory</b>, o código garante a criação de instâncias com todas as dependências configuradas corretamente.</p>

### **Camada Core**

<ul>
  <li><b>Modelos e DTOs</b>: contém as definições das entidades que representam os dados de cronogramas e tratamentos capilares. Além disso, os DTOs (Data Transfer Objects) garantem a transferência eficiente de dados entre o backend e o frontend.</li>
  <li><b>Serviços</b>: As regras de negócios são implementadas nos serviços, que seguem os princípios SOLID. Estes serviços são consumidos pelos controladores na camada <b>API</b>.</li>
</ul>

### **Camada API**

<ul>
  <li><b>Controladores</b>: expondo os endpoints da API para gerenciar os cronogramas, controladores como <code>ScheduleController</code> permitem a comunicação entre o cliente (frontend) e os dados do banco.</li>
  <li><b>Program e Middleware</b>: o ponto de entrada da aplicação está no arquivo <code>Program.cs</code>, e o middleware é configurado para tratar requisições e respostas HTTP.</li>
</ul>

## 🛠 Design da Aplicação

<p>O design da aplicação segue os princípios de <b>Clean Architecture</b>, onde a separação de preocupações é primordial. A camada <b>Core</b> lida com a lógica de negócios, enquanto a camada <b>Infra</b> gerencia a persistência e acesso a dados. Isso garante que o código seja modular e extensível, facilitando a manutenção e escalabilidade do sistema.</p>

<p>Além disso, o uso do padrão <b>Factory</b> ajuda na criação e gerenciamento de objetos complexos, garantindo que o código seja fácil de entender e manter, com todos os componentes devidamente organizados e desacoplados.</p>
