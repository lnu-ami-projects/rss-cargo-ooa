from diagrams import Diagram, Cluster, Edge
from diagrams.programming.language import Csharp
from diagrams.onprem.database import PostgreSQL
from diagrams.onprem.client import Users
from diagrams.custom import Custom
from urllib.request import urlretrieve

with Diagram("RSSCargo Web Application Architecture", show=False):
    user = Users("Web User")

    dotnet_url = (
        "https://upload.wikimedia.org/wikipedia/commons/0/0e/Microsoft_.NET_logo.png"
    )
    dotnet_icon = "dotnet.png"
    urlretrieve(dotnet_url, dotnet_icon)

    blazor_url = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d0/Blazor.png/600px-Blazor.png"
    blazor_icon = "blazor.png"
    urlretrieve(blazor_url, blazor_icon)

    with Cluster("RSSCargo Web Application"):

        with Cluster("Controllers"):
            controllers = Custom(
                "HomeController\nOrderController\nCargoController", dotnet_icon
            )

        with Cluster("Models"):
            models = Csharp("Order\nCargo\nUser")

        with Cluster("Views"):
            views = Custom("Razor Pages", blazor_icon)

        with Cluster("Data Access"):
            db_context = Custom("ApplicationDbContext", dotnet_icon)

        with Cluster("Services"):
            services = Custom("Business Logic Services", dotnet_icon)

        user >> Edge(label="HTTP Requests") >> controllers
        controllers >> Edge(label="Processes") >> views
        controllers >> Edge(label="Uses") >> services
        services >> Edge(label="Accesses") >> models
        models >> Edge(label="Queries") >> db_context
        db_context >> Edge(label="Reads/Writes") >> PostgreSQL("PostgreSQL Database")
