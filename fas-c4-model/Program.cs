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
            
            //SUSCRIPCION

            Component SubscriptionController = SubscriptionContext.AddComponent("Subscription Controller", "", "Spring Boot REST Controller");
            Component ValidationComponent = SubscriptionContext.AddComponent("Validation Component Service", " ", "Spring Component");
            Component PaymentVerifier = SubscriptionContext.AddComponent("Payment Verifier", "", "Spring Component");
            Component SubscriptionRepository = SubscriptionContext.AddComponent("Subscription Repository", "", "Spring Component");
            Component ServiceComponent = SubscriptionContext.AddComponent("Service Component", "", "Spring Component");

            apiGateway.Uses(SubscriptionController, "", "JSON/HTTPS");
            SubscriptionController.Uses(ValidationComponent, "Validates Prompts");
            SubscriptionController.Uses(ServiceComponent, "Usa");
            ValidationComponent.Uses(PaymentVerifier, "", "JDBC");
            PaymentVerifier.Uses(SubscriptionContextDatabase, "", "JDBC");
            SubscriptionRepository.Uses(SubscriptionContextDatabase, "", "JDBC");
            PaymentVerifier.Uses(PayPal, "", "JSON/HTTPS");
            ServiceComponent.Uses(SubscriptionRepository, "", "JSON/HTTPS");
   
            SubscriptionController.AddTags("SubscriptionController");
            ValidationComponent.AddTags("ValidationComponent");
            PaymentVerifier.AddTags("PaymentVerifier");
            SubscriptionRepository.AddTags("SubscriptionRepository");
            ServiceComponent.AddTags("ServiceComponent");

            styles.Add(new ElementStyle("SubscriptionController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ValidationComponent") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentVerifier") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SubscriptionRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ServiceComponent") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(SubscriptionContext, "Components", "Component Diagram Subscription");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiGateway);
            componentView.Add(SubscriptionContextDatabase);
            componentView.Add(PayPal);
            componentView.AddAllComponents();
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);


            //USER PROFILE
            Component SignIn = UserProfileContext.AddComponent("Sign In Controller", "", "NestJS");
            Component LogIn = UserProfileContext.AddComponent("Log In Controller", "", "NestJS(NodeJS) REST Controller");
            Component ValidationComponentU = UserProfileContext.AddComponent("Validation Component", "", "NestJS component");
            Component LawyerProfile = UserProfileContext.AddComponent("Lawyer Profile Repository", "", "NestJS comonent");
            Component UserProfileRep = UserProfileContext.AddComponent("User Profile Repository", "", "NestJS component");

            apiGateway.Uses(SignIn, "", "JSON/HTTPS");
            apiGateway.Uses(LogIn, "", "JSON/HTTPS");
            SignIn.Uses(ValidationComponentU, "", "Validates Prompts");
            LogIn.Uses(ValidationComponentU, "", "Validates Prompts");
            ValidationComponentU.Uses(LawyerProfile, "", "JSON/HTTPS");
            ValidationComponentU.Uses(UserProfileRep, "", "JSON/HTTPS");
            LawyerProfile.Uses(UserProfileContextDatabase, "", "JDBC");
            UserProfileRep.Uses(UserProfileContextDatabase, "", "JDBC");

            SignIn.AddTags("SignIn");
            LogIn.AddTags("LogIn");
            ValidationComponentU.AddTags("ValidateComponentU");
            LawyerProfile.AddTags("LawyerProfile");
            UserProfileRep.AddTags("UserProfileRep");

            styles.Add(new ElementStyle("SignIn") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LogIn") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ValidateComponentU") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LawyerProfile") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("UserProfileRep") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView2 = viewSet.CreateComponentView(UserProfileContext, "Components 2", "Component Diagram User Profile");
            componentView2.PaperSize = PaperSize.A4_Landscape;
            componentView2.Add(mobileApplication);
            componentView2.Add(webApplication);
            componentView2.Add(apiGateway);
            componentView2.Add(UserProfileContextDatabase);
            componentView2.AddAllComponents();
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
            
            
            //QUALIFICATION
            Component QualificationController = QualificationContext.AddComponent("Qualification Controller", "", "NestJS");
            Component QualificationValidation = QualificationContext.AddComponent("Qualification Validation Component", "", "NestJS");
            Component QualificationRepository = QualificationContext.AddComponent("Qualification Repository", "", "NestJS");
            Component QServiceComponent = QualificationContext.AddComponent("Qualification Service Component", "", "NestJS");

            apiGateway.Uses(QualificationController, "", "JSON/HTTPS");
            QualificationController.Uses(QServiceComponent, "", "JSON/HTTPS");
            QualificationController.Uses(QualificationValidation, "", "Validation Prompts");
            QServiceComponent.Uses(QualificationRepository, "", "JSON/HTTPS");
            QualificationRepository.Uses(QualificationContextDatabase, "", "JDBC");
            QualificationValidation.Uses(QualificationRepository, "", "JSON/HTTPS");

            QualificationController.AddTags("QualificationController");
            QualificationValidation.AddTags("QualificationValidation");
            QualificationRepository.AddTags("QualificationRepository");
            QServiceComponent.AddTags("QServiceComponent");

            styles.Add(new ElementStyle("QualificationController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("QualificationValidation") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("QualificationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("QServiceComponent") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView3 = viewSet.CreateComponentView(QualificationContext, "Components 3", "Component Diagram Qualification Context");
            componentView3.PaperSize = PaperSize.A4_Landscape;
            componentView3.Add(mobileApplication);
            componentView3.Add(webApplication);
            componentView3.Add(apiGateway);
            componentView3.Add(QualificationContextDatabase);
            componentView3.AddAllComponents();
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
            
             //SERVICES
            Component ServicesController = ServicesContext.AddComponent("Services Controller", "", "REST API");
            Component ConsultationService = ServicesContext.AddComponent("Consultation Service", "", "Spring component");
            Component ServiceValidation = ServicesContext.AddComponent("Sevice Validation", "", "NestJS component");
            Component ConsultationPaymentVerifier = ServicesContext.AddComponent("Consultation Payment Verifier", "", "NestJS component");
            Component ConsultationPaymentVerifierRepo = ServicesContext.AddComponent("Consultation Payment Verifier Repository", "", "Spring component");
            Component ConsultationServiceRepo = ServicesContext.AddComponent("Consultation Service Lawyer Repository", "", "Spring component");

            apiGateway.Uses(ServicesController, "JSON/HTTPS");
            ServicesController.Uses(ConsultationService, "invoca servicios de");
            ConsultationService.Uses(ServiceValidation, "JSON/HTTPS");
            ConsultationService.Uses(ConsultationServiceRepo, "JDBC");
            ConsultationService.Uses(GoogleMeets, "JSON/HTTPS");
            ServiceValidation.Uses(ConsultationPaymentVerifier, "Json/HTTPS");
            ServiceValidation.Uses(LinkedIn, "Json/HTTPS");
            ConsultationPaymentVerifier.Uses(ConsultationPaymentVerifierRepo, "JDBC");
            ConsultationPaymentVerifier.Uses(PayPal, "Json/HTTPS");
            ConsultationPaymentVerifierRepo.Uses(ServicesContextDatabase, "JDBC");
            ConsultationServiceRepo.Uses(ServicesContextDatabase, "JDBC");

            ServicesController.AddTags("ServicesController");
            ConsultationService.AddTags("ConsultationService");
            ServiceValidation.AddTags("ServiceValidation");
            ConsultationPaymentVerifier.AddTags("ConsultationPaymentVerifier");
            ConsultationPaymentVerifierRepo.AddTags("ConsultationPaymentVerifierRepo");
            ConsultationServiceRepo.AddTags("ConsultationServiceRepo");

            styles.Add(new ElementStyle("ServicesController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ConsultationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ServiceValidation") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ConsultationPaymentVerifier") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ConsultationPaymentVerifierRepo") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ConsultationServiceRepo") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView4 = viewSet.CreateComponentView(ServicesContext, "Components 4", "Component Diagram Services Context");
            componentView4.PaperSize = PaperSize.A4_Landscape;
            componentView4.Add(mobileApplication);
            componentView4.Add(webApplication);
            componentView4.Add(apiGateway);
            componentView4.Add(ServicesContextDatabase);
            componentView4.Add(GoogleMeets);
            componentView4.Add(LinkedIn);
            componentView4.Add(PayPal);
            componentView4.AddAllComponents();
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);

        

        }
    }
}
