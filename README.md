# GPS Intersect

This application displays a GPS route (GPX or KML) on a map and calculates the timestamps of intersections with areas represented by polygons.

It was developed in Visual Studio 2022 using the following technologies:

* C# / WPF Application
* MVVM Light Toolkit
* XML / JSON Serialization
* CefSharp Browser
* Open Street Map
* Open Layers

Here is a screenshot of the application running:

<img src='https://drive.google.com/uc?id=10HW2hevK_BxcGgBzcSrSTMVkf854L7c0' width='240'>

I have provided examples of GPS files in both formats and a polygon map which can be opened from the executable folder. You can create your own polygon maps online using the Google Earth application.

If you need to convert between the file types there is a converter found in the Tools menu.

More information on the KML file format can be found here:

https://developers.google.com/kml/documentation/kml_tut

The GPX file format is documented here:

https://docs.fileformat.com/gis/gpx/

# Note

You may need to change the Build Configuration to Release and x64 Platform for the project to build.
