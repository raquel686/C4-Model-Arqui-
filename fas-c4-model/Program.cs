using Structurizr;
using Structurizr.Api;

namespace fas_c4_model
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 73415;
            const string apiKey = "5d43bc49-acbf-4d92-88a8-812c389c8c08";
            const string apiSecret = "5b68cf8a-8784-4b14-88de-1d3a8e9031b8";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("FAS C4 Model - Sistema de software de Legiz", "Sistema de software de Legiz");
            Model model = workspace.Model;
            ViewSet viewSet = workspace.Views;

            // 1. Diagrama de Contexto
            SoftwareSystem LegizSistem = model.AddSoftwareSystem("Sistema de software de Legiz", "Permite el correcto funcionamiento de nuestra plataforma de ayuda a clientes por parte de abogados");
            SoftwareSystem LinkedIn = model.AddSoftwareSystem("LinkedIn", "");
            SoftwareSystem GoogleMeets = model.AddSoftwareSystem("Google Meets", "");
            SoftwareSystem PayPal = model.AddSoftwareSystem("PayPal", "");

            Person cliente = model.AddPerson("Cliente", "Usuario de la app.");
            Person abogado = model.AddPerson("Abogado", "Graduado de la facultad de letras.");
           

            cliente.Uses(LegizSistem, "Se conecta al software para adquirir un servicio de representación o solución de preguntas.");
            abogado.Uses(LegizSistem, "Soluciona o responde las preguntas del usuario y en caso se haya solicitado, lo representa.");

            LegizSistem.Uses(GoogleMeets, "Usa para las conexiones o encuentros entre usuario y abogado");
            LegizSistem.Uses(LinkedIn, "Consulta y valida la profesion y experiencia de nuestros socios abogados");
            LegizSistem.Uses(PayPal, "Administra los pagos que recibimos y otorgamos");

            SystemContextView contextView = viewSet.CreateSystemContextView(LegizSistem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            LegizSistem.AddTags("SistemaMonitoreo");
            LinkedIn.AddTags("LinkedIn");
            GoogleMeets.AddTags("GoogleMeets");
            PayPal.AddTags("PayPal");
            cliente.AddTags("cliente");
            abogado.AddTags("abogado");
           
            
            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("cliente") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("abogado") { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
           
            styles.Add(new ElementStyle("SistemaMonitoreo") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("LinkedIn") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("GoogleMeets") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("PayPal") { Background = "#ffe169", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            Container mobileApplication = LegizSistem.AddContainer("Mobile App", "Permite a los usuarios visualizar, consultar y solicitar los servicios de leyes que ofrecemos.", "Flutter");
            Container webApplication = LegizSistem.AddContainer("Web App", "Permite a los usuarios visualizar, consultar y solicitar los servicios de leyes que ofrecemos.", "Flutter Web");
            Container landingPage = LegizSistem.AddContainer("Landing Page", "", "Flutter Web");
            Container apiGateway = LegizSistem.AddContainer("API Gateway", "API Gateway", "NodeJS (NestJS) port 8080");
            Container QualificationContext = LegizSistem.AddContainer("Qualification Context", "Bounded Context del Microservicio de Calificaciones", "Spring Boot port 8081");
            Container SecurityContext = LegizSistem.AddContainer("Security Context", "Bounded Context del Microservicio de búsqueda de abogados", "Spring Boot port 8082");
            Container UserProfileContext = LegizSistem.AddContainer("User Profile Context", "Bounded Context del Microservicio de perfil de usuario", "Spring Boot port 8083");
            Container SubscriptionContext = LegizSistem.AddContainer("Subscription Context", "Bounded Context del Microservicio de subscripciones", "Spring Boot port 8084");
            Container ServicesContext = LegizSistem.AddContainer("Services Context", "Bounded Context del Microservicio de conjunto de servicios", "Spring Boot port 8085");
            Container messageBus =
                LegizSistem.AddContainer("Bus de Mensajes en Cluster de Alta Disponibilidad", "Transporte de eventos del dominio.", "Azure");
            Container QualificationContextDatabase = LegizSistem.AddContainer("Qualification Context DB", "", "MySql");
            Container SecurityContextDatabase = LegizSistem.AddContainer("Search Lawyer Context DB", "", "MySql");
            Container UserProfileContextDatabase = LegizSistem.AddContainer("User Profile Context DB", "", "MySql");
            Container SubscriptionContextDatabase = LegizSistem.AddContainer("Subscription Context DB", "", "MySql");
            Container ServicesContextDatabase = LegizSistem.AddContainer("Services Context DB", "", "MySql");
            Container ServicesContextReplicaDatabase = LegizSistem.AddContainer("Services Context DB Replica", "", "MySql");
            //Container ServicesContextReactiveDatabase = LegizSistem.AddContainer("Monitoring Context Reactive DB", "", "Firebase Cloud Firestore");
            
            cliente.Uses(mobileApplication, "Consulta");
            cliente.Uses(webApplication, "Consulta");
            cliente.Uses(landingPage, "Consulta");
            abogado.Uses(mobileApplication, "Consulta");
            abogado.Uses(webApplication, "Consulta");
            abogado.Uses(landingPage, "Consulta");
            mobileApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiGateway, "API Request", "JSON/HTTPS");
            
            apiGateway.Uses(QualificationContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(SecurityContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(UserProfileContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(SubscriptionContext, "API Request", "JSON/HTTPS");
            apiGateway.Uses(ServicesContext, "API Request", "JSON/HTTPS");
            QualificationContext.Uses(messageBus, "Publica y consume eventos del dominio");
            QualificationContext.Uses(QualificationContextDatabase, "", "JDBC");
            SecurityContext.Uses(messageBus, "Publica y consume eventos del dominio");
            SecurityContext.Uses(SecurityContextDatabase, "", "JDBC");
            UserProfileContext.Uses(messageBus, "Publica y consume eventos del dominio");
            UserProfileContext.Uses(UserProfileContextDatabase, "", "JDBC");
            SubscriptionContext.Uses(messageBus, "Publica y consume eventos del dominio");
            SubscriptionContext.Uses(SubscriptionContextDatabase, "", "JDBC");
            ServicesContext.Uses(messageBus, "Publica y consume eventos del dominio");
            ServicesContext.Uses(ServicesContextDatabase, "", "JDBC");
            ServicesContext.Uses(ServicesContextReplicaDatabase, "", "JDBC");
            //ServicesContext.Uses(ServicesContextReactiveDatabase, "", "");
            ServicesContextDatabase.Uses(ServicesContextReplicaDatabase, "Replica");
            ServicesContext.Uses(LinkedIn, "API Request", "JSON/HTTPS");
            ServicesContext.Uses(GoogleMeets, "API Request", "JSON/HTTPS");
            ServicesContext.Uses(PayPal, "API Request", "JSON/HTTPS");
            SubscriptionContext.Uses(PayPal, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiGateway.AddTags("APIGateway");
            QualificationContext.AddTags("QualificationContext");
            QualificationContextDatabase.AddTags("QualificationContextDatabase");
            SecurityContext.AddTags("SecurityContext");
            SecurityContextDatabase.AddTags("SecurityContextDatabase");
            UserProfileContext.AddTags("UserProfileContext");
            UserProfileContextDatabase.AddTags("UserProfileContextDatabase");
            SubscriptionContext.AddTags("SubscriptionContext");
            SubscriptionContextDatabase.AddTags("SubscriptionContextDatabase");
            ServicesContext.AddTags("ServicesContext");
            ServicesContextDatabase.AddTags("ServicesContextDatabase");
            ServicesContextReplicaDatabase.AddTags("ServicesContextReplicaDatabase");
            //ServicesContextReactiveDatabase.AddTags("ServicesContextReactiveDatabase");
            messageBus.AddTags("MessageBus");
            
            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#e9c7ff", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIGateway") { Shape = Shape.RoundedBox, Background = "#0cc41b", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("QualificationContext") { Shape = Shape.Hexagon, Background = "#f77028", Icon = "" });
            styles.Add(new ElementStyle("QualificationContextDatabase") { Shape = Shape.Cylinder, Background = "#e8e84a", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("SecurityContext") { Shape = Shape.Hexagon, Background = "#f77028", Icon = "" });
            styles.Add(new ElementStyle("SecurityContextDatabase") { Shape = Shape.Cylinder, Background = "#e8e84a", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("UserProfileContext") { Shape = Shape.Hexagon, Background = "#f77028", Icon = "" });
            styles.Add(new ElementStyle("UserProfileContextDatabase") { Shape = Shape.Cylinder, Background = "#e8e84a", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("SubscriptionContext") { Shape = Shape.Hexagon, Background = "#f77028", Icon = "" });
            styles.Add(new ElementStyle("SubscriptionContextDatabase") { Shape = Shape.Cylinder, Background = "#e8e84a", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("ServicesContext") { Shape = Shape.Hexagon, Background = "#f77028", Icon = "" });
            styles.Add(new ElementStyle("ServicesContextDatabase") { Shape = Shape.Cylinder, Background = "#e8e84a", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("ServicesContextReplicaDatabase") { Shape = Shape.Cylinder, Background = "#e8e84a", Color = "#ffffff", Icon = "" });
            //styles.Add(new ElementStyle("ServicesContextReactiveDatabase") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("MessageBus") { Width = 850, Background = "#e84a5d", Color = "#ffffff", Shape = Shape.Pipe, Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(LegizSistem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();


            // 3. Diagrama de Componentes
           
            Component SubscriptionController = ServicesContext.AddComponent("Subscription Controller", "", "Spring Boot REST Controller");
            Component ValidationComponent = ServicesContext.AddComponent("Validation Component Service", " ", "Spring Component");
            
            Component PaymentVerifier = ServicesContext.AddComponent("Payment Verifier", "", "Spring Component");
            Component SubscriptionRepository = ServicesContext.AddComponent("Subscription Repository", "", "Spring Component");
            Component ServiceComponent = ServicesContext.AddComponent("Service Component", "", "Spring Component");

            apiGateway.Uses(SubscriptionController, "", "JSON/HTTPS");
            SubscriptionController.Uses(ValidationComponent, "Validates Prompts");
            SubscriptionController.Uses(ServiceComponent, "Usa");
            
       
            ValidationComponent.Uses(PaymentVerifier, "", "JDBC");
           
            
            PaymentVerifier.Uses(ServicesContextDatabase, "", "JDBC");
            SubscriptionRepository.Uses(ServicesContextDatabase, "", "JDBC");
       
            PaymentVerifier.Uses(LinkedIn, "", "JSON/HTTPS");
            ServiceComponent.Uses(SubscriptionRepository, "", "JSON/HTTPS");
            

            // Tags
            
            SubscriptionController.AddTags("SubscriptionController");
            ValidationComponent.AddTags("ValidationComponent");
            
            PaymentVerifier.AddTags("PaymentVerifier");
            SubscriptionRepository.AddTags("SubscriptionRepository");
            ServiceComponent.AddTags("ServiceComponent");

            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SubscriptionController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ValidationComponent") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            
            styles.Add(new ElementStyle("PaymentVerifier") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SubscriptionRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ServiceComponent") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(ServicesContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiGateway);
            componentView.Add(ServicesContextDatabase);
          
            componentView.Add(LinkedIn);
            componentView.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);

            /*
            Component domainLayer = ServicesContext.AddComponent("Domain Layer", "", "Spring Boot");
            Component monitoringController = ServicesContext.AddComponent("Monitoring Controller", "REST API endpoints de monitoreo.", "Spring Boot REST Controller");
            Component monitoringApplicationService = ServicesContext.AddComponent("Monitoring Application Service", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "Spring Component");
            Component flightRepository = ServicesContext.AddComponent("Flight Repository", "Información del vuelo", "Spring Component");
            Component vaccineLoteRepository = ServicesContext.AddComponent("VaccineLote Repository", "Información de lote de vacunas", "Spring Component");
            Component locationRepository = ServicesContext.AddComponent("Location Repository", "Ubicación del vuelo", "Spring Component");
            Component aircraftSystemFacade = ServicesContext.AddComponent("Aircraft System Facade", "", "Spring Component");

            apiGateway.Uses(monitoringController, "", "JSON/HTTPS");
            monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");
            monitoringController.Uses(aircraftSystemFacade, "Usa");
            monitoringApplicationService.Uses(domainLayer, "Usa", "");
            monitoringApplicationService.Uses(flightRepository, "", "JDBC");
            monitoringApplicationService.Uses(vaccineLoteRepository, "", "JDBC");
            monitoringApplicationService.Uses(locationRepository, "", "JDBC");
            flightRepository.Uses(ServicesContextDatabase, "", "JDBC");
            vaccineLoteRepository.Uses(ServicesContextDatabase, "", "JDBC");
            locationRepository.Uses(ServicesContextDatabase, "", "JDBC");

            locationRepository.Uses(LinkedIn, "", "JSON/HTTPS");
            aircraftSystemFacade.Uses(GoogleMeets, "JSON/HTTPS");

            // Tags
            domainLayer.AddTags("DomainLayer");
            monitoringController.AddTags("MonitoringController");
            monitoringApplicationService.AddTags("MonitoringApplicationService");
            flightRepository.AddTags("FlightRepository");
            vaccineLoteRepository.AddTags("VaccineLoteRepository");
            locationRepository.AddTags("LocationRepository");
            aircraftSystemFacade.AddTags("AircraftSystemFacade");

            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(ServicesContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiGateway);
            componentView.Add(ServicesContextDatabase);
            componentView.Add(GoogleMeets);
            componentView.Add(LinkedIn);
            componentView.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
            */

        }
    }
}