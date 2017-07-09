Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls

Public Module RouteConfig
    Public Sub RegisterRoutes(routes As RouteCollection)
        'The next 3 lines provide mobile view behaviour - Uncomment and delete the fourth one in case to enable mobile switch mode
        'Dim settings = New FriendlyUrlSettings()
        'settings.AutoRedirectMode = RedirectMode.Permanent
        'routes.EnableFriendlyUrls(settings)
        routes.EnableFriendlyUrls()
    End Sub
End Module
