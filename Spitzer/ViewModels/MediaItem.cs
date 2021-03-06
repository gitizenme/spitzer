﻿// MIT License
//
// Copyright (c) [2020] [Joe Chavez]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using Spitzer.Models.ImageMetadata;
using Spitzer.Models.Images;

namespace Spitzer.Models.NasaMedia
{
    public partial class MediaItem
    {
        public string Title => Data.Select(d => d.Title).FirstOrDefault() ?? string.Empty;
        public string DateCreated => Data.Select(d => d.DateCreated).FirstOrDefault().ToLocalTime().ToString("MM/dd/yyyy hh:mm tt");
        public long DateSort => Data.Select(d => d.DateCreated).FirstOrDefault().ToLocalTime().ToUnixTimeSeconds();
        public string Description => Data.Select(d => d.Description).FirstOrDefault() ?? string.Empty;
        public string Description508 => Data.Select(d => d.Description508).FirstOrDefault() ?? string.Empty;
        public Uri ImagePreview => Links.Select(l => l.Href).FirstOrDefault();

        public IList<Uri> Images
        {
            get
            {
                var client = new RestClient("https://images-assets.nasa.gov");

                var nasaId = Data.Select(d => d.NasaId).FirstOrDefault();
                if(nasaId != null)
                {
                    var request = new RestRequest($"/image/{nasaId}/collection.json", Method.GET);

                    var jsonImageList = client.Execute(request);
                    return ImageAssets.FromJson(jsonImageList.Content);
                }
                var previewImage = new List<Uri>
                {
                    ImagePreview
                };
                return previewImage;
            }
        }

        public ImageMetaData MetaData
        {
            get
            {
                // https://images-assets.nasa.gov/image/PIA11796/metadata.json
                var client = new RestClient("https://images-assets.nasa.gov");

                var nasaId = Data.Select(d => d.NasaId).FirstOrDefault();
                if (nasaId != null)
                {
                    var request = new RestRequest($"/image/{nasaId}/metadata.json", Method.GET);

                    var data = client.Execute(request);
                    return ImageMetaData.FromJson(data.Content);
                }
                return null;
            }
        }
    }


    /*
     * Href contains a link to JSON file:
     * https://images-assets.nasa.gov/image/PIA11796/collection.json
     * ["http://images-assets.nasa.gov/image/PIA11796/PIA11796~orig.jpg", "http://images-assets.nasa.gov/image/PIA11796/PIA11796~large.jpg", "http://images-assets.nasa.gov/image/PIA11796/PIA11796~medium.jpg", "http://images-assets.nasa.gov/image/PIA11796/PIA11796~small.jpg", "http://images-assets.nasa.gov/image/PIA11796/PIA11796~thumb.jpg", "http://images-assets.nasa.gov/image/PIA11796/metadata.json"]
     */

    /*
     * {
    "collection": {
        "metadata": {
            "total_hits": 500
        },
        "href": "https://images-api.nasa.gov/search?q=Spitzer%20space%20telescope&keywords=spitzer%20space%20telescope&media_type=image",
        "links": [
            {
                "rel": "next",
                "prompt": "Next",
                "href": "https://images-api.nasa.gov/search?page=2&keywords=spitzer+space+telescope&q=Spitzer+space+telescope&media_type=image"
            }
        ],
        "version": "1.0",
        "items": [
            {
                "href": "https://images-assets.nasa.gov/image/PIA11796/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11796/PIA11796~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The galaxy Messier 101 is a swirling spiral of stars, gas, and dust. Messier 101 is nearly twice as wide as our Milky Way galaxy in this image as seen by NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/STScI",
                        "title": "Spitzer Space Telescope View of Galaxy Messier 101",
                        "description": "The galaxy Messier 101 is a swirling spiral of stars, gas, and dust. Messier 101 is nearly twice as wide as our Milky Way galaxy in this image as seen by NASA Spitzer Space Telescope.",
                        "date_created": "2009-02-10T13:52:01Z",
                        "keywords": [
                            "Messier 101",
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11796",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA22089/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA22089/PIA22089~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist's concept showcases both the visible and infrared visualizations of the Orion Nebula, looking down a 'valley' leading to the star cluster at far end.",
                        "center": "JPL",
                        "secondary_creator": "NNASA/ESA, F. Summers, G. Bacon, Z. Levay, J. DePasquale, L. Frattare, M. Robberto and M. Gennaro (STScI), and R. Hurt (Caltech/IPAC)",
                        "title": "Hubble Space Telescope,Spitzer Space Telescope",
                        "description": "This image showcases both the visible and infrared visualizations of the Orion Nebula. This view from a movie sequence looks down the 'valley' leading to the star cluster at the far end. The left side of the image shows the visible-light visualization, which fades to the infrared-light visualization on the right. These two contrasting models derive from observations by the Hubble and Spitzer space telescopes.  An animation is available at https://photojournal.jpl.nasa.gov/catalog/PIA22089",
                        "date_created": "2018-01-11T00:00:00Z",
                        "keywords": [
                            "Hubble Space Telescope",
                            "Spitzer Space Telescope",
                            "Orion Nebula"
                        ],
                        "nasa_id": "PIA22089",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04497/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04497/PIA04497~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image was used in a contest to rename the Space InfraRed Telescope Facility SIRTF, now known as the Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL",
                        "title": "Help Stamp Out Boring Space Acronyms",
                        "description": "This image was used in a contest to rename the Space InfraRed Telescope Facility SIRTF, now known as the Spitzer Space Telescope.",
                        "date_created": "2003-05-15T23:40:29Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04497",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17444/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17444/PIA17444~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist concept shows NASA Spitzer Space Telescope surrounded by examples of exoplanets the telescope has examined in over its ten years in space.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Spitzer Trains Its Eyes on Exoplanets Artist Concept",
                        "description": "This artist concept shows NASA Spitzer Space Telescope surrounded by examples of exoplanets the telescope has examined in over its ten years in space.",
                        "date_created": "2013-09-24T20:30:27Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17444",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08007/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08007/PIA08007~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Galaxy NGC 4579 was captured by the Spitzer Infrared Nearby Galaxy Survey, or Sings, Legacy project using the Spitzer Space Telescope infrared array camera. I",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Ariz",
                        "title": "Galaxy NGC 4579",
                        "description": "Galaxy NGC 4579 was captured by the Spitzer Infrared Nearby Galaxy Survey, or Sings, Legacy project using the Spitzer Space Telescope infrared array camera. I",
                        "date_created": "2006-02-15T00:51:52Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA08007",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13290/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13290/PIA13290~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist image illustrates vibrating buckyballs -- spherical molecules of carbon discovered in space for the first time by NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Buckyballs Jiggle Like Jello  Artist Concept",
                        "description": "This artist image illustrates vibrating buckyballs -- spherical molecules of carbon discovered in space for the first time by NASA Spitzer Space Telescope. ",
                        "date_created": "2010-07-22T18:00:02Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13290",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04265/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04265/PIA04265~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Technicians put final touches on NASA Space Infrared Telescope Facility at Lockheed Martin Aeronautics in Sunnyvale, Calif., which launched on August 25, 2003. The telescope is now known as the Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA",
                        "title": "Finishing Touches for Space Infrared Telescope Facility SIRTF",
                        "description": "Technicians put final touches on NASA Space Infrared Telescope Facility at Lockheed Martin Aeronautics in Sunnyvale, Calif., which launched on August 25, 2003. The telescope is now known as the Spitzer Space Telescope.",
                        "date_created": "2003-03-19T19:16:36Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04265",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA02587/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA02587/PIA02587~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The false-color composite image of the Stephan\u2019s Quintet galaxy cluster is made up of data from NASA Spitzer Space Telescope and a ground-based telescope in Spain.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Max Planck Institute",
                        "title": "A Shocking Surprise in Stephan Quintet",
                        "description": "The false-color composite image of the Stephan\u2019s Quintet galaxy cluster is made up of data from NASA Spitzer Space Telescope and a ground-based telescope in Spain.",
                        "date_created": "2006-03-03T16:49:04Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA02587",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA03031/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA03031/PIA03031~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has captured this stunning infrared view of the famous galaxy Messier 31, also known as Andromeda.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Ariz.<br />Figure 3: NOAO/AURA/NSF",
                        "title": "Amazing Andromeda in Red",
                        "description": "NASA Spitzer Space Telescope has captured this stunning infrared view of the famous galaxy Messier 31, also known as Andromeda.",
                        "date_created": "2005-10-13T17:20:44Z",
                        "keywords": [
                            "M31",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA03031",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04267/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04267/PIA04267~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope whizzes in front of a brilliant, infrared view of the Milky Way galaxy plane in this artistic depiction.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL",
                        "title": "SST and the Milky Way, an Artist Concept",
                        "description": "NASA Spitzer Space Telescope whizzes in front of a brilliant, infrared view of the Milky Way galaxy plane in this artistic depiction.",
                        "date_created": "2003-03-22T22:03:28Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04267",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04937/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04937/PIA04937~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The magnificent spiral arms of the nearby galaxy Messier 81 are highlighted in this image from NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech/University of Arizona/Harvard-Smithsonian Center for Astrophysics/NOAO/AURA/NSF",
                        "title": "Multi-Wavelength Views of Messier 81",
                        "description": "The magnificent spiral arms of the nearby galaxy Messier 81 are highlighted in this image from NASA Spitzer Space Telescope. ",
                        "date_created": "2003-12-18T18:14:58Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04937",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17256/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17256/PIA17256~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The spectacular swirling arms and central bar of the Sculptor galaxy are revealed in this new view from NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "The Barred Sculptor Galaxy",
                        "description": "The spectacular swirling arms and central bar of the Sculptor galaxy are revealed in this new view from NASA Spitzer Space Telescope.",
                        "date_created": "2013-08-23T17:05:18Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17256",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA20069/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA20069/PIA20069~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The varying brightness of an exoplanet called 55 Cancri e is shown in this plot of infrared data captured by NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/University of Cambridge",
                        "title": "Hot N Hotter Planet Measured by Spitzer",
                        "description": "The varying brightness of an exoplanet called 55 Cancri e is shown in this plot of infrared data captured by NASA Spitzer Space Telescope.",
                        "date_created": "2016-03-30T18:26:02Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA20069",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18905/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18905/PIA18905~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The famous Horsehead nebula takes on a ghostly appearance in this image from NASA Spitzer Space Telescope, released on December 18, 2014.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Horsehead of a Different Color",
                        "description": "The famous Horsehead nebula takes on a ghostly appearance in this image from NASA Spitzer Space Telescope, released on December 18, 2014.",
                        "date_created": "2014-12-19T18:29:43Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18905",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13900/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13900/PIA13900~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This view of the Sunflower galaxy highlights a variety of infrared wavelengths captured by NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Spitzer Sunflower Galaxy",
                        "description": "This view of the Sunflower galaxy highlights a variety of infrared wavelengths captured by NASA Spitzer Space Telescope.",
                        "date_created": "2011-03-03T23:08:49Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13900",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18453/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18453/PIA18453~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image of asteroid 2011 MD was taken by NASA Spitzer Space Telescope in Feb. 2014, over a period of 20 hours.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Northern Arizona University/SAO",
                        "title": "I Spy a Little Asteroid With My Infrared Eye",
                        "description": "This image of asteroid 2011 MD was taken by NASA Spitzer Space Telescope in Feb. 2014, over a period of 20 hours.",
                        "date_created": "2014-06-19T16:45:28Z",
                        "keywords": [
                            "Asteroid",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18453",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10711/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10711/PIA10711~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope imaged the mysterious ring around magnetar SGR 1900+14 in infrared light.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Ghostly Ring",
                        "description": "NASA Spitzer Space Telescope imaged the mysterious ring around magnetar SGR 1900+14 in infrared light.",
                        "date_created": "2008-05-28T15:10:00Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA10711",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA05736/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA05736/PIA05736~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows an exceptionally bright source of radio emission called DR21.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Star Formation in the DR21 Region B",
                        "description": "This image from NASA Spitzer Space Telescope shows an exceptionally bright source of radio emission called DR21.",
                        "date_created": "2004-04-13T17:58:00Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA05736",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04941/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04941/PIA04941~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has detected the building blocks of life in the distant universe, albeit in a violent milieu. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech",
                        "title": "Spectrum from Faint Galaxy IRAS F00183-7111",
                        "description": "NASA Spitzer Space Telescope has detected the building blocks of life in the distant universe, albeit in a violent milieu. ",
                        "date_created": "2003-12-18T18:15:25Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04941",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09561/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09561/PIA09561~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Two large elliptical galaxies, NGC 4889 and NGC 4874 are shown in this image from NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/GSFC/SDSS",
                        "title": "Dwarfs in Coma Cluster",
                        "description": "Two large elliptical galaxies, NGC 4889 and NGC 4874 are shown in this image from NASA Spitzer Space Telescope.",
                        "date_created": "2007-05-29T21:38:18Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09561",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04938/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04938/PIA04938~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The magnificent and dusty spiral arms of the nearby galaxy Messier 81 are highlighted in these NASA Spitzer Space Telescope images. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech/University of Arizona",
                        "title": "Long-Wavelength Infrared Views of Messier 81",
                        "description": "The magnificent and dusty spiral arms of the nearby galaxy Messier 81 are highlighted in these NASA Spitzer Space Telescope images. ",
                        "date_created": "2003-12-18T18:15:08Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04938",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA19332/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA19332/PIA19332~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This infographic explains how NASA Spitzer Space Telescope can be used in tandem with a telescope on the ground to measure the distances to planets discovered using the microlensing technique.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Infographic: Finding Planets With Microlensing",
                        "description": "This infographic explains how NASA Spitzer Space Telescope can be used in tandem with a telescope on the ground to measure the distances to planets discovered using the microlensing technique.  http://photojournal.jpl.nasa.gov/catalog/PIA19332",
                        "date_created": "2015-04-14T15:52:02Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA19332",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13929/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13929/PIA13929~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image layout shows two views of the same baby star from NASA Spitzer Space Telescope. Spitzer view shows that this star has a second, identical jet shooting off in the opposite direction of the first.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Undercover Jet Exposed",
                        "description": "This image layout shows two views of the same baby star from NASA Spitzer Space Telescope. Spitzer view shows that this star has a second, identical jet shooting off in the opposite direction of the first.",
                        "date_created": "2011-04-04T22:30:01Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13929",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11390/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11390/PIA11390~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This figure charts 30 hours of observations taken by NASA Spitzer Space Telescope of a strongly irradiated exoplanet an planet orbiting a star beyond our own. Spitzer measured changes in the planet heat, or infrared light.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/UCSC",
                        "title": "Light from Red-Hot Planet",
                        "description": "This figure charts 30 hours of observations taken by NASA Spitzer Space Telescope of a strongly irradiated exoplanet an planet orbiting a star beyond our own. Spitzer measured changes in the planet heat, or infrared light.",
                        "date_created": "2009-01-28T16:59:01Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11390",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA01903/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA01903/PIA01903~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows the scattered remains of an exploded star named Cassiopeia A. Spitzer infrared detectors picked through these remains and found that much of the star original layering had been preserved. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Lighting up a Dead Star Layers",
                        "description": "This image from NASA Spitzer Space Telescope shows the scattered remains of an exploded star named Cassiopeia A. Spitzer infrared detectors picked through these remains and found that much of the star original layering had been preserved. ",
                        "date_created": "2006-10-26T17:57:52Z",
                        "keywords": [
                            "Cassiopeia A",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA01903",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13901/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13901/PIA13901~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows infrared light from the Sunflower galaxy, otherwise known as Messier 63. Spitzer view highlights the galaxy dusty spiral arms.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Sunflower Galaxy Glows with Infrared Light",
                        "description": "This image from NASA Spitzer Space Telescope shows infrared light from the Sunflower galaxy, otherwise known as Messier 63. Spitzer view highlights the galaxy dusty spiral arms.",
                        "date_created": "2011-03-03T23:08:50Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13901",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09925/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09925/PIA09925~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This composite image shows the Coronet in X-rays from Chandra and infrared from NASA Spitzer Space Telescope orange, green, and cyan. The Spitzer data show young stars plus diffuse emission from dust.",
                        "center": "JPL",
                        "secondary_creator": "NASA/CXC/JPL-Caltech/CfA",
                        "title": "Coronet: A Star-Formation Neighbor",
                        "description": "This composite image shows the Coronet in X-rays from Chandra and infrared from NASA Spitzer Space Telescope orange, green, and cyan. The Spitzer data show young stars plus diffuse emission from dust.",
                        "date_created": "2007-09-13T15:46:23Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09925",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA23123/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA23123/PIA23123~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This deep-field view of the sky taken by NASA's Hubble and Spitzer space telescopes is dominated by galaxies.",
                        "media_type": "image",
                        "secondary_creator": "NASA/JPL-Caltech/ESA/Spitzer/P. Oesch/S. De Barros/ I.Labbe",
                        "title": "A Field of Galaxies Seen by Spitzer and Hubble",
                        "description": "This deep-field view of the sky, taken by NASA's Spitzer Space Telescope, is dominated by galaxies - including some very faint, very distant ones - circled in red. The bottom right inset shows one of those distant galaxies, made visible thanks to a long-duration observation by Spitzer. The wide-field view also includes data from NASA's Hubble Space Telescope. The Spitzer observations came from the GREATS survey, short for GOODS Re-ionization Era wide-Area Treasury from Spitzer. GOODS is itself an acronym: Great Observatories Origins Deep Survey.  https://photojournal.jpl.nasa.gov/catalog/PIA23123",
                        "date_created": "2019-05-08T00:00:00Z",
                        "keywords": [
                            "Spitzer Space Telescope",
                            "Hubble Space Telescope"
                        ],
                        "nasa_id": "PIA23123",
                        "center": "JPL"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13316/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13316/PIA13316~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image of two tangled galaxies has been released by NASA Great Observatories. The Antennae galaxies are shown in this composite image from the Chandra X-ray Observatory, the Hubble Space Telescope, and the Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/CXC/SAO/JPL-Caltech/STScI",
                        "title": "NASA Great Observatories Witness a Galactic Spectacle",
                        "description": "This image of two tangled galaxies has been released by NASA Great Observatories. The Antennae galaxies are shown in this composite image from the Chandra X-ray Observatory, the Hubble Space Telescope, and the Spitzer Space Telescope.",
                        "date_created": "2010-08-05T17:15:55Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13316",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09200/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09200/PIA09200~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist concept, based on spectral observations from NASA Hubble Space Telescope and Spitzer Space Telescope, shows a cloudy Jupiter-like planet that orbits very close to its fiery hot star.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Exotic Atmospheres  Artist Concept",
                        "description": "This artist concept, based on spectral observations from NASA Hubble Space Telescope and Spitzer Space Telescope, shows a cloudy Jupiter-like planet that orbits very close to its fiery hot star.",
                        "date_created": "2007-02-21T17:50:04Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09200",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA22918/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA22918/PIA22918~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The location of Gaia 17bpi, which lies in the Sagitta constellation, is indicated in this image taken by NASA's Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/M. Kuhn (Caltech)",
                        "title": "Star Gaia 17pbi Seen by Spitzer",
                        "description": "The location of Gaia 17bpi, which lies in the Sagitta constellation, is indicated in this image taken by NASA's Spitzer Space Telescope.  https://photojournal.jpl.nasa.gov/catalog/PIA22918",
                        "date_created": "2018-12-18T00:00:00Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA22918",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA19331/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA19331/PIA19331~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This plot shows data obtained from NASA Spitzer Space Telescope, and the Optical Gravitational Lensing Experiment telescope located in Chile, during a microlensing event.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Warsaw University Observatory",
                        "title": "Time Delay in Microlensing Event",
                        "description": "This plot shows data obtained from NASA's Spitzer Space Telescope and the Optical Gravitational Lensing Experiment, or OGLE, telescope located in Chile, during a \"microlensing\" event. Microlensing events occur when one star passes another, and the gravity of the foreground star causes the distant star's light to magnify and brighten. This magnification is evident in the plot, as both Spitzer and OGLE register an increase in the star's brightness.  If the foreground star is circled by a planet, the planet's gravity can alter the magnification over a shorter period, seen in the plot in the form of spikes and a dip. The great distance between Spitzer, in space, and OGLE, on the ground, meant that Spitzer saw this particular microlensing event before OGLE. The offset in the timing can be used to measure the distance to the planet.  In this case, the planet, called OGLE-2014-BLG-0124L, was found to be 13,000 light-years away, near the center of our Milky Way galaxy.  The finding was the result of fortuitous timing because Spitzer's overall program to observe microlensing events was only just starting up in the week before the planet's effects were visible from Spitzer's vantage point.  While Spitzer sees infrared light of 3.6 microns in wavelength, OGLE sees visible light of 0.8 microns.  http://photojournal.jpl.nasa.gov/catalog/PIA19331",
                        "date_created": "2015-04-14T15:52:01Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA19331",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09109/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09109/PIA09109~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image composite highlights the pillars of the Eagle nebula, as seen in infrared light by NASA Spitzer Space Telescope bottom and visible light by NASA Hubble Space Telescope top insets.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/STScI/ Institut dAstrophysique Spatiale",
                        "title": "Unwrapping the Pillars",
                        "description": "This image composite highlights the pillars of the Eagle nebula, as seen in infrared light by NASA Spitzer Space Telescope bottom and visible light by NASA Hubble Space Telescope top insets.",
                        "date_created": "2007-01-09T17:56:03Z",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09109",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13134/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13134/PIA13134~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope contributed to the infrared component of the observations of a surprisingly large collections of galaxies red dots in center. Shorter-wavelength infrared and visible data are provided by Japan Subaru telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Subaru",
                        "title": "Galactic Metropolis",
                        "description": "NASA Spitzer Space Telescope contributed to the infrared component of the observations of a surprisingly large collections of galaxies red dots in center. Shorter-wavelength infrared and visible data are provided by Japan Subaru telescope.",
                        "date_created": "2010-05-11T21:58:03Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13134",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10932/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10932/PIA10932~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The green and red splotch in this image is the most active star-making galaxy in the very distant universe. Nicknamed Baby Boom, it was spotted 12.3 billion light-years away by a suite of telescopes, including NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Subaru/STScI ",
                        "title": "Super Starburst Galaxy",
                        "description": "The green and red splotch in this image is the most active star-making galaxy in the very distant universe. Nicknamed Baby Boom, it was spotted 12.3 billion light-years away by a suite of telescopes, including NASA Spitzer Space Telescope. ",
                        "date_created": "2008-07-10T16:52:19Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA10932",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA07395/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA07395/PIA07395~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This spectrum shows the light from a dusty, distant galaxy located 11 billion light-years away. The galaxy is invisible to optical telescopes, but NASA Spitzer Space Telescope captured the light from it and dozens of other similar galaxies.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Cornell",
                        "title": "Fingerprints in the Light",
                        "description": "This spectrum shows the light from a dusty, distant galaxy located 11 billion light-years away. The galaxy is invisible to optical telescopes, but NASA Spitzer Space Telescope captured the light from it and dozens of other similar galaxies.",
                        "date_created": "2005-03-01T17:48:59Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA07395",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA01936/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA01936/PIA01936~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist animation shows a blistering world revolving around its nearby un. NASA infrared Spitzer Space Telescope observed a planetary system like this one, as the planet sunlit and dark hemispheres swung alternately into the telescope view.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Fire and Ice Planet  Artist Concept",
                        "description": "This artist animation shows a blistering world revolving around its nearby un. NASA infrared Spitzer Space Telescope observed a planetary system like this one, as the planet sunlit and dark hemispheres swung alternately into the telescope view.",
                        "date_created": "2006-10-12T17:33:44Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA01936",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09071/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09071/PIA09071~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This is a composite image of N49, the brightest supernova remnant in optical light in the Large Magellanic Cloud; the image combines data from the Chandra X-ray Telescope blue and NASA Spitzer Space Telescope red.",
                        "center": "JPL",
                        "secondary_creator": "NASA/CXC/STScI/JPL-Caltech/UIUC/Univ. of Minn.",
                        "title": "Stellar Debris in the Large Magellanic Cloud",
                        "description": "This is a composite image of N49, the brightest supernova remnant in optical light in the Large Magellanic Cloud; the image combines data from the Chandra X-ray Telescope blue and NASA Spitzer Space Telescope red.",
                        "date_created": "2006-12-08T23:56:13Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09071",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13789/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13789/PIA13789~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Maffei 2 is the poster child for an infrared galaxy that is almost invisible to optical telescopes. But this infrared image from NASA Spitzer Space Telescope penetrates the dust to reveal the galaxy in all its glory.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/UCLA",
                        "title": "The Hidden Galaxy",
                        "description": "Maffei 2 is the poster child for an infrared galaxy that is almost invisible to optical telescopes. But this infrared image from NASA Spitzer Space Telescope penetrates the dust to reveal the galaxy in all its glory.",
                        "date_created": "2011-01-18T23:15:41Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13789",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04943/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04943/PIA04943~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has captured an image of an unusual comet that experiences frequent outbursts, which produce abrupt changes in brightness.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech/Ames Research Center/University of Arizona",
                        "title": "Comet Schwassmann-Wachmann I",
                        "description": "NASA Spitzer Space Telescope has captured an image of an unusual comet that experiences frequent outbursts, which produce abrupt changes in brightness.",
                        "date_created": "2003-12-18T18:15:38Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04943",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17257/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17257/PIA17257~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Massive stars can wreak havoc on their surroundings, as can be seen in this new view of the Carina nebula from NASAs Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "The Tortured Clouds of Eta Carinae",
                        "description": "Massive stars can wreak havoc on their surroundings, as can be seen in this new view of the Carina nebula from NASAs Spitzer Space Telescope.",
                        "date_created": "2013-08-23T17:05:18Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17257",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12257/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12257/PIA12257~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Keck",
                        "title": "Saturn Infrared Ring  Artist Concept",
                        "description": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. ",
                        "date_created": "2009-10-07T17:41:38Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA12257",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12377/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12377/PIA12377~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows two young brown dwarfs, objects that fall somewhere between planets and stars in terms of their temperature and mass.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Calar Alto Obsv./Caltech Sub. Obsv.",
                        "title": "Twin Brown Dwarfs Wrapped in a Blanket",
                        "description": "This image from NASA Spitzer Space Telescope shows two young brown dwarfs, objects that fall somewhere between planets and stars in terms of their temperature and mass.",
                        "date_created": "2009-11-23T00:00:02Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA12377",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13551/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13551/PIA13551~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "An infrared photo of the Small Magellanic Cloud taken by NASA Spitzer Space Telescope is shown in this artist illustration; an example of a planetary nebula, and a magnified depiction of buckyballs.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Extragalactic Space Balls Artist Concept",
                        "description": "An infrared photo of the Small Magellanic Cloud taken by NASA Spitzer Space Telescope is shown in this artist illustration; an example of a planetary nebula, and a magnified depiction of buckyballs.",
                        "date_created": "2010-10-27T18:30:45Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13551",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10019/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10019/PIA10019~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Astronomers using NASA Spitzer Space Telescope found evidence that such quasar winds might have forged these dusty particles in the very early universe. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Dust in the Quasar Wind Artist Concept",
                        "description": "Astronomers using NASA Spitzer Space Telescope found evidence that such quasar winds might have forged these dusty particles in the very early universe. ",
                        "date_created": "2007-10-09T17:26:24Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA10019",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11173/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11173/PIA11173~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "A jet of gas firing out of a very young star can be seen ramming into a wall of material in this infrared image from NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Harvard-Smithsonian CfA",
                        "title": "Laser-Sharp Jet Splits Water",
                        "description": "A jet of gas firing out of a very young star can be seen ramming into a wall of material in this infrared image from NASA Spitzer Space Telescope. ",
                        "date_created": "2008-09-18T20:41:24Z",
                        "keywords": [
                            "Perseus",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11173",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11445/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11445/PIA11445~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has captured a new, infrared view of the choppy star-making cloud called M17, or the Swan nebula.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Wisc.",
                        "title": "Celestial Sea of Stars",
                        "description": "NASA Spitzer Space Telescope has captured a new, infrared view of the choppy star-making cloud called M17, or the Swan nebula. ",
                        "date_created": "2008-12-08T17:39:44Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11445",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18909/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18909/PIA18909~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This series of images show three evolutionary phases of massive star formation, as pictured in infrared images from NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Evolution of a Massive Star",
                        "description": "This series of images show three evolutionary phases of massive star formation, as pictured in infrared images from NASA Spitzer Space Telescope.",
                        "date_created": "2015-01-27T18:14:13Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18909",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13149/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13149/PIA13149~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image is one of six images taken by NASA Spitzer Space Telescope, showing that tight-knit twin, or binary stars might be triggered to form by asymmetrical envelopes.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Univ. of Michigan",
                        "title": "Blobs House Twin Stars",
                        "description": "This image is one of six images taken by NASA Spitzer Space Telescope, showing that tight-knit twin, or binary stars might be triggered to form by asymmetrical envelopes.",
                        "date_created": "2010-05-20T20:55:09Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13149",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA14100/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA14100/PIA14100~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Using NASA Spitzer Space Telescope, astronomers have, for the first time, found signatures of silicate crystals around a newly forming protostar in the constellation of Orion. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/University of Toledo",
                        "title": "Cosmic Fountain of Crystal Rain",
                        "description": "Using NASA Spitzer Space Telescope, astronomers have, for the first time, found signatures of silicate crystals around a newly forming protostar in the constellation of Orion. ",
                        "date_created": "2011-05-26T15:55:07Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA14100",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09338/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09338/PIA09338~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Two rambunctious young stars are destroying their natal dust cloud with powerful jets of radiation, in an infrared image from NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Harvard-Smithsonian CfA",
                        "title": "Spitzer Digs Up Hidden Stars",
                        "description": "Two rambunctious young stars are destroying their natal dust cloud with powerful jets of radiation, in an infrared image from NASA Spitzer Space Telescope. ",
                        "date_created": "2007-05-01T19:35:20Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09338",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10955/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10955/PIA10955~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows he Peony nebula star, a blazing ball of gas shines with the equivalent light of 3.2 million suns.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Potsdam Univ.",
                        "title": "Peony Nebula Star Settles for Silver Medal",
                        "description": "This image from NASA Spitzer Space Telescope shows he Peony nebula star, a blazing ball of gas shines with the equivalent light of 3.2 million suns.",
                        "date_created": "2008-07-15T16:55:28Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA10955",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA03605/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA03605/PIA03605~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This false-color infrared image from NASA Spitzer Space Telescope shows little dwarf galaxies forming in the tails of two larger galaxies that are colliding together.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Cornell Univ.",
                        "title": "Dwarf Galaxies Swimming in Tidal Tails",
                        "description": "This false-color infrared image from NASA Spitzer Space Telescope shows little dwarf galaxies forming in the tails of two larger galaxies that are colliding together.",
                        "date_created": "2005-12-01T20:16:12Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA03605",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04942/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04942/PIA04942~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has obtained the first infrared images of the dust disc surrounding Fomalhaut, the 18th brightest star in the sky.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech/Maxwell Telescope",
                        "title": "Circumstellar Disk Around Fomalhaut",
                        "description": "NASA Spitzer Space Telescope has obtained the first infrared images of the dust disc surrounding Fomalhaut, the 18th brightest star in the sky.",
                        "date_created": "2003-12-18T18:15:35Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04942",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15624/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15624/PIA15624~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This graphic illuminates the process by which astronomers using NASA Spitzer Space Telescope have, for the first time, detected the light from a super Earth planet.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Measuring Brightness of Super Earth 55 Cancri e",
                        "description": "This graphic illuminates the process by which astronomers using NASA Spitzer Space Telescope have, for the first time, detected the light from a super Earth planet.",
                        "date_created": "2012-05-08T18:30:04Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA15624",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15808/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15808/PIA15808~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Astronomers using NASA Spitzer Space Telescope have detected what they believe is an alien world just two-thirds the size of Earth -- one of the smallest on record.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Exoplanet is Extremely Hot and Incredibly Close Artist Concept",
                        "description": "Astronomers using NASA Spitzer Space Telescope have detected what they believe is an alien world just two-thirds the size of Earth -- one of the smallest on record.",
                        "date_created": "2012-07-18T17:00:05Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA15808",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11228/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11228/PIA11228~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope captured this picture of comet Holmes in March 2008, five months after the comet suddenly erupted and brightened a millionfold overnight.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Anatomy of a Busted Comet",
                        "description": "NASA Spitzer Space Telescope captured this picture of comet Holmes in March 2008, five months after the comet suddenly erupted and brightened a millionfold overnight.",
                        "date_created": "2008-10-13T17:08:42Z",
                        "keywords": [
                            "Holmes",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11228",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17259/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17259/PIA17259~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The locations of brown dwarfs discovered by NASA Wide-field Infrared Survey Explorer, or WISE, and mapped by NASA Spitzer Space Telescope, are shown in this diagram as red circles.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Brown Dwarf Backyardigans",
                        "description": "The locations of brown dwarfs discovered by NASA Wide-field Infrared Survey Explorer, or WISE, and mapped by NASA Spitzer Space Telescope, are shown in this diagram as red circles.",
                        "date_created": "2013-09-05T19:43:37Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17259",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18908/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18908/PIA18908~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Yellow balls -- which are several hundred to thousands times the size of our solar system -- are pictured here in the center of this image taken by NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Finding Yellowballs in our Milky Way",
                        "description": "Yellow balls -- which are several hundred to thousands times the size of our solar system -- are pictured here in the center of this image taken by NASA Spitzer Space Telescope.",
                        "date_created": "2015-01-27T18:14:11Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18908",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04939/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04939/PIA04939~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope transforms a dark cloud into a silky translucent veil, revealing the molecular outflow from an otherwise hidden newborn star. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech",
                        "title": "Embedded Outflows from Herbig-Haro 46/47",
                        "description": "This image from NASA Spitzer Space Telescope transforms a dark cloud into a silky translucent veil, revealing the molecular outflow from an otherwise hidden newborn star. ",
                        "date_created": "2003-12-18T18:15:12Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04939",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA20066/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA20066/PIA20066~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The turbulent atmosphere of a hot, gaseous planet known as HD 80606b is shown in this simulation based on data from NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/MIT/Principia College",
                        "title": "Simulated Atmosphere of a Hot Gas Giant",
                        "description": "The turbulent atmosphere of a hot, gaseous planet known as HD 80606b is shown in this simulation based on data from NASA Spitzer Space Telescope.",
                        "date_created": "2016-03-28T15:22:50Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA20066",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17443/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17443/PIA17443~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "With the help of NASA Spitzer Space Telescope, astronomers have discovered that what was thought to be a large asteroid called Don Quixote is in fact a comet.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/DLR/NAU",
                        "title": "Spitzer Spies a Comet Coma and Tail",
                        "description": "With the help of NASA Spitzer Space Telescope, astronomers have discovered that what was thought to be a large asteroid called Don Quixote is in fact a comet.",
                        "date_created": "2013-09-10T20:13:49Z",
                        "keywords": [
                            "Comet",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17443",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18454/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18454/PIA18454~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Observations of infrared light from NASA Spitzer Space Telescope coming from asteroids provide a better estimate of their true sizes than visible-light measurements.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "How to Measure the Size of an Asteroid",
                        "description": "Observations of infrared light from NASA Spitzer Space Telescope coming from asteroids provide a better estimate of their true sizes than visible-light measurements.",
                        "date_created": "2014-06-19T21:01:06Z",
                        "keywords": [
                            "Asteroid",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18454",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17843/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17843/PIA17843~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The red arc in this infrared image from NASA Spitzer Space Telescope is a giant shock wave, created by a speeding star known as Kappa Cassiopeiae.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Speedster Star Shocks the Galaxy",
                        "description": "The red arc in this infrared image from NASA Spitzer Space Telescope is a giant shock wave, created by a speeding star known as Kappa Cassiopeiae.",
                        "date_created": "2014-02-20T23:35:00Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17843",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09108/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09108/PIA09108~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This set of images from NASA Spitzer Space Telescope shows the Eagle nebula in different hues of infrared light. Each view tells a different tale. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/ Institut dAstrophysique Spatiale",
                        "title": "Eagle Nebula Flaunts its Infrared Feathers",
                        "description": "This set of images from NASA Spitzer Space Telescope shows the Eagle nebula in different hues of infrared light. Each view tells a different tale. ",
                        "date_created": "2007-01-09T17:56:02Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09108",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12256/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12256/PIA12256~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "The King of Rings  Artist Concept",
                        "description": "This artist conception shows a nearly invisible ring around Saturn -- the largest of the giant planet many rings. It was discovered by NASA Spitzer Space Telescope. ",
                        "date_created": "2009-10-07T17:41:37Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA12256",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09070/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09070/PIA09070~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope left panel shows the bow shock of a dying star named R Hydrae, or R Hya, in the constellation Hydra.",
                        "center": "JPL",
                        "secondary_creator": "NASA/CXC/STScI/JPL-Caltech/UIUC/Univ. of Minn.",
                        "title": "Red Giant Plunging Through Space",
                        "description": "This image from NASA Spitzer Space Telescope left panel shows the bow shock of a dying star named R Hydrae, or R Hya, in the constellation Hydra.",
                        "date_created": "2006-12-08T23:45:46Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09070",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04934/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04934/PIA04934~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope image of a glowing stellar nursery provides a spectacular contrast to the opaque cloud seen in visible light inset. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech",
                        "title": "Dark Globule in IC 1396 IRAC",
                        "description": "NASA Spitzer Space Telescope image of a glowing stellar nursery provides a spectacular contrast to the opaque cloud seen in visible light inset. ",
                        "date_created": "2003-12-18T18:14:46Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04934",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA04724/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA04724/PIA04724~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This engineering image derives from 100 seconds of observing time on one of the three science instruments aboard the Space InfraRed Telescope Facility SIRTF now known as the Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL/Caltech",
                        "title": "SIRTF Aliveness",
                        "description": "This engineering image derives from 100 seconds of observing time on one of the three science instruments aboard the Space InfraRed Telescope Facility (SIRTF). SIRTF was launched on August 25, and opened up its focal plane to starlight on August 30. This image was obtained as part of the instrument power-on sequence on September 1, one week after launch and a full month before the telescope is expected to reach optimal operating temperature and focus. The stars and galaxies seen in this image already attest to the observatory's great sensitivity in the infrared and to its proper operation.  The Space Infrared Telescope Facility (SIRTF) telescope was renamed the Spitzer Space Telescope on December 18, 2003, after the late Dr. Lyman Spitzer, in a contest which was open to the general public.   http://photojournal.jpl.nasa.gov/catalog/PIA04724",
                        "date_created": "2003-09-03T19:22:24Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA04724",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15423/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15423/PIA15423~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "From left to right, artist concepts of the Spitzer, Planck and Kepler space telescopes. NASA extended Spitzer and Kepler for two additional years; and the U.S. portion of Planck, a European Space Agency mission, for one year.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Spitzer, Planck and Kepler Extended by NASA Artist Concept",
                        "description": "From left to right, artist concepts of the Spitzer, Planck and Kepler space telescopes. NASA extended Spitzer and Kepler for two additional years; and the U.S. portion of Planck, a European Space Agency mission, for one year.",
                        "date_created": "2012-04-05T20:50:44Z",
                        "keywords": [
                            "Kepler,Planck,Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA15423",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA16690/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA16690/PIA16690~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer and Hubble space telescopes have teamed up to uncover a mysterious infant star that behaves like a police strobe light.",
                        "center": "JPL",
                        "secondary_creator": "NASA/ESA/JPL-Caltech/STScI/NOAO/University of Arizona/ Max Planck Institute for Astronomy/University of Massachusetts, Amherst",
                        "title": "Protostar LRLL 54361",
                        "description": "NASA Spitzer and Hubble space telescopes have teamed up to uncover a mysterious infant star that behaves like a police strobe light.",
                        "date_created": "2013-02-07T18:14:17Z",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA16690",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA16612/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA16612/PIA16612~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This graph shows the brightness variations of the brown dwarf named 2MASSJ22282889-431026 measured simultaneously by both NASA Hubble and Spitzer space telescopes.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/University of Arizona",
                        "title": "Probing Brown Dwarf Layers",
                        "description": "This graph shows the brightness variations of the brown dwarf named 2MASSJ22282889-431026 measured simultaneously by both NASA Hubble and Spitzer space telescopes.",
                        "date_created": "2013-01-08T22:28:12Z",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA16612",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17847/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17847/PIA17847~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The closest supernova of its kind to be observed in the last few decades, M82 or the Cigar galaxy, has sparked a global observing campaign involving legions of instruments on the ground and in space, including NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Carnegie Institution for Science",
                        "title": "Seeing Through a Veil of Dust",
                        "description": "The closest supernova of its kind to be observed in the last few decades, M82 or the Cigar galaxy, has sparked a global observing campaign involving legions of instruments on the ground and in space, including NASA Spitzer Space Telescope.",
                        "date_created": "2014-02-26T15:47:14Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17847",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18455/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18455/PIA18455~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Observations from NASA Spitzer Space Telescope, taken in infrared light, have helped to reveal that a small asteroid called 2011 MD is made-up of two-thirds empty space.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Solid as a Rock? Porosity of Asteroids",
                        "description": "Observations from NASA Spitzer Space Telescope, taken in infrared light, have helped to reveal that a small asteroid called 2011 MD is made-up of two-thirds empty space.",
                        "date_created": "2014-06-19T21:02:06Z",
                        "keywords": [
                            "Asteroid",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18455",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13289/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13289/PIA13289~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image illustrates that buckyballs -- discovered in space by NASA Spitzer Space Telescope -- closely resemble old fashioned, black-and-white soccer balls, only on much smaller scales. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Mini Soccer Balls in Space",
                        "description": "This image illustrates that buckyballs -- discovered in space by NASA Spitzer Space Telescope -- closely resemble old fashioned, black-and-white soccer balls, only on much smaller scales. ",
                        "date_created": "2010-07-22T18:00:01Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13289",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13287/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13287/PIA13287~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "These data from NASA Spitzer Space Telescope show the signatures of buckyballs in space. Buckyballs, also called C60 or buckministerfullerenes, after architect Buckminister Fuller geodesic domes.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/University of Western Ontario",
                        "title": "Jiggling Soccer-Ball Molecules in Space",
                        "description": "These data from NASA Spitzer Space Telescope show the signatures of buckyballs in space. Buckyballs, also called C60 or buckministerfullerenes, after architect Buckminister Fuller geodesic domes.",
                        "date_created": "2010-07-22T17:59:58Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13287",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09100/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09100/PIA09100~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This is an image from NASA Spitzer Space Telescope of stars and galaxies in the Ursa Major constellation. This infrared image covers a region of space so large that light would take up to 100 million years to travel across it. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/GSFC",
                        "title": "The Universe First Fireworks",
                        "description": "This is an image from NASA Spitzer Space Telescope of stars and galaxies in the Ursa Major constellation. This infrared image covers a region of space so large that light would take up to 100 million years to travel across it. ",
                        "date_created": "2006-12-18T17:55:09Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09100",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11392/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11392/PIA11392~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows a computer simulation of the planet HD 80606b from an observer located at a point in space lying between the Earth and the HD 80606 system. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/UCSC",
                        "title": "Tour of Planet with Extreme Temperature Swings",
                        "description": "This image from NASA Spitzer Space Telescope shows a computer simulation of the planet HD 80606b from an observer located at a point in space lying between the Earth and the HD 80606 system. ",
                        "date_created": "2009-01-28T16:59:03Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11392",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15266/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15266/PIA15266~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has detected the solid form of buckyballs in space for the first time. To form a solid particle, the buckyballs must stack together, as illustrated in this artist concept showing the very beginnings of the process.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Building a Buckyball Particle in Space Artist Concept",
                        "description": "NASA Spitzer Space Telescope has detected the solid form of buckyballs in space for the first time. To form a solid particle, the buckyballs must stack together, as illustrated in this artist concept showing the very beginnings of the process.",
                        "date_created": "2012-02-22T15:58:07Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA15266",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA03244/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA03244/PIA03244~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Newborn stars, hidden behind thick dust, are revealed in this image of a section of the Christmas Tree cluster from NASA Spitzer Space Telescope, created in joint effort between Spitzer infrared array camera and multiband imaging photometer instrument",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/CfA",
                        "title": "Stellar Snowflake Cluster",
                        "description": "Newborn stars, hidden behind thick dust, are revealed in this image of a section of the Christmas Tree cluster from NASA Spitzer Space Telescope, created in joint effort between Spitzer infrared array camera and multiband imaging photometer instrument",
                        "date_created": "2005-12-22T22:10:44Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA03244",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13930/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13930/PIA13930~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope took this image of a baby star sprouting two identical jets green lines emanating from fuzzy star. The left jet was hidden behind a dark cloud, which Spitzer can see through.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "It Twins! Spitzer Finds Hidden Jet",
                        "description": "NASA Spitzer Space Telescope took this image of a baby star sprouting two identical jets green lines emanating from fuzzy star. The left jet was hidden behind a dark cloud, which Spitzer can see through.",
                        "date_created": "2011-04-04T22:30:02Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13930",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13303/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13303/PIA13303~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "A star-forming region shines from the considerable distance of more than 30,000 light-years away in the upper left of this image from NASA Spitzer Space Telescope. This image is a combination of data from Spitzer and the Two Micron All Sky Survey.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/2MASS/SSI/University of Wisconsin",
                        "title": "Beastly Stars and a Bubble",
                        "description": "A star-forming region shines from the considerable distance of more than 30,000 light-years away in the upper left of this image from NASA Spitzer Space Telescope. This image is a combination of data from Spitzer and the Two Micron All Sky Survey.",
                        "date_created": "2010-07-28T21:07:46Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13303",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA12151/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA12151/PIA12151~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has imaged a wild creature of the dark -- a coiled galaxy with an eye-like object at its center.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Coiled Creature of the Night",
                        "description": "NASA Spitzer Space Telescope has imaged a wild creature of the dark -- a coiled galaxy with an eye-like object at its center.",
                        "date_created": "2009-07-23T17:00:14Z",
                        "keywords": [
                            "NGC 1097",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA12151",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09962/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09962/PIA09962~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "A newly expanded image of the Helix nebula lends a festive touch to the fourth anniversary of the launch of NASA Spitzer Space Telescope",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Spitzer Celebrates Fourth Anniversary with Celestial Fireworks",
                        "description": "A newly expanded image of the Helix nebula lends a festive touch to the fourth anniversary of the launch of NASA Spitzer Space Telescope",
                        "date_created": "2007-08-24T17:19:19Z",
                        "keywords": [
                            "Helix Nebula",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09962",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08509/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08509/PIA08509~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Astronomers have discovered nearly 300 galaxy clusters and groups, including almost 100 located 8 to 10 billion light-years away, using the space-based Spitzer Space Telescope and the ground-based Mayall 4-meter telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/NOAO",
                        "title": "Galaxies Gather at Great Distances",
                        "description": "Astronomers have discovered nearly 300 galaxy clusters and groups, including almost 100 located 8 to 10 billion light-years away, using the space-based Spitzer Space Telescope and the ground-based Mayall 4-meter telescope.",
                        "date_created": "2006-06-05T16:55:30Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA08509",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11227/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11227/PIA11227~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This painterly portrait of a star-forming cloud, called NGC 346, is a combination of multiwavelength light from NASA Spitzer Space Telescope, the European Southern Observatory New Technology Telescope, and the European Space Agency.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/ESA/ESO/MPIA",
                        "title": "Stellar Work of Art",
                        "description": "This painterly portrait of a star-forming cloud, called NGC 346, is a combination of multiwavelength light from NASA Spitzer Space Telescope, the European Southern Observatory New Technology Telescope, and the European Space Agency.",
                        "date_created": "2008-10-08T17:46:41Z",
                        "keywords": [
                            "NGC 346",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11227",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13288/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13288/PIA13288~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope has at last found buckyballs resembling soccer balls in space shown in this artist concept using Hubble picture of the NGC 2440 nebula. Hubble image cred: NASA, ESA, STScI",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Space Balls Artist Concept",
                        "description": "NASA Spitzer Space Telescope has at last found buckyballs resembling soccer balls in space shown in this artist concept using Hubble picture of the NGC 2440 nebula. Hubble image cred: NASA, ESA, STScI",
                        "date_created": "2010-07-22T17:59:59Z",
                        "keywords": [
                            "Hubble Space Telescope,Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13288",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA02180/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA02180/PIA02180~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist concept based on data fromNASA Spitzer Space Telescope shows delicate greenish crystals sprinkled throughout the violent core of a pair of colliding galaxies. The white spots represent a thriving population of stars of all sizes and ages.",
                        "center": "JPL",
                        "secondary_creator": "Artist Concept: NASA/JPL-Caltech<br />Graph: NASA/JPL-Caltech/Cornell",
                        "title": "Galactic Hearts of Glass  Artist Concept",
                        "description": "This artist concept based on data fromNASA Spitzer Space Telescope shows delicate greenish crystals sprinkled throughout the violent core of a pair of colliding galaxies. The white spots represent a thriving population of stars of all sizes and ages.",
                        "date_created": "2006-02-15T17:50:51Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA02180",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA15819/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA15819/PIA15819~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This graph illustrates the Cepheid period-luminosity relationship, used to calculate the size, age and expansion rate of the universe. The data shown are from NASA Spitzer Space Telescope which has made the most precise measurements yet.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Cepheids as Cosmology Tools",
                        "description": "This graph illustrates the Cepheid period-luminosity relationship, used to calculate the size, age and expansion rate of the universe. The data shown are from NASA Spitzer Space Telescope which has made the most precise measurements yet.",
                        "date_created": "2012-10-03T17:15:14Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA15819",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA17996/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA17996/PIA17996~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This mosaic reveals a panorama of the Milky Way from NASA Spitzer Space Telescope. This picture covers only about three percent of the sky, but includes more than half of the galaxy stars and the majority of its star formation activity.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/University of Wisconsin",
                        "title": "GLIMPSE the Galaxy All the Way Around",
                        "description": "This mosaic reveals a panorama of the Milky Way from NASA Spitzer Space Telescope. This picture covers only about three percent of the sky, but includes more than half of the galaxy stars and the majority of its star formation activity.",
                        "date_created": "2014-03-20T19:55:22Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA17996",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18010/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18010/PIA18010~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Astronomers have found cosmic clumps so dark, dense and dusty that they throw the deepest shadows ever recorded. A large cloud looms in the center of this image of the galactic plane from NASA Spitzer Space Telescope.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/University of Zurich",
                        "title": "Mapping the Densest Dusty Cloud Cores",
                        "description": "Astronomers have found cosmic clumps so dark, dense and dusty that they throw the deepest shadows ever recorded. A large cloud looms in the center of this image of the galactic plane from NASA Spitzer Space Telescope.",
                        "date_created": "2014-05-21T17:55:41Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18010",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA13494/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA13494/PIA13494~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This frame from an animation based on NASA Spitzer Space Telescope data illustrates an unexpected warm spot on the surface of a gaseous exoplanet.The bright orange patches are the hottest part of the planet.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Weird Warm Spot on Exoplanet",
                        "description": "This frame from an animation based on NASA Spitzer Space Telescope data illustrates an unexpected warm spot on the surface of a gaseous exoplanet.The bright orange patches are the hottest part of the planet.",
                        "date_created": "2010-10-19T17:00:53Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA13494",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA10119/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA10119/PIA10119~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "A rare, infrared view of a developing star and its flaring jets taken by NASA Spitzer Space Telescope shows us what our own solar system might have looked like billions of years ago. ",
                        "center": "JPL",
                        "secondary_creator": "IRAC image: NASA/JPL-Caltech/UIUC<br />  Visible image: NASA/JPL-Caltech/AURA",
                        "title": "Baby Picture of our Solar System",
                        "description": "A rare, infrared view of a developing star and its flaring jets taken by NASA Spitzer Space Telescope shows us what our own solar system might have looked like billions of years ago. ",
                        "date_created": "2007-11-29T17:55:23Z",
                        "keywords": [
                            "L1157",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA10119",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11047/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11047/PIA11047~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Generations of stars can be seen in this new infrared portrait from NASA Spitzer Space Telescope. In this wispy star-forming region, called W5, the oldest stars can be seen as blue dots in the centers of the two hollow cavities.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/Harvard-Smithsonian CfA",
                        "title": "Spitzer Reveals Stellar Family Tree",
                        "description": "Generations of stars can be seen in this new infrared portrait from NASA Spitzer Space Telescope. In this wispy star-forming region, called W5, the oldest stars can be seen as blue dots in the centers of the two hollow cavities.",
                        "date_created": "2008-08-22T16:04:54Z",
                        "keywords": [
                            "W5",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11047",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA11213/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA11213/PIA11213~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The Cassiopeia A supernova first flash of radiation makes six clumps of dust circled in annotated version unusually hot. The supernova remnant is the large white ball in the center. This infrared picture was taken by NASA Spitzer Space Telescope. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/E. Dwek and R. Arendt",
                        "title": "Supernova Flashback",
                        "description": "The Cassiopeia A supernova first flash of radiation makes six clumps of dust circled in annotated version unusually hot. The supernova remnant is the large white ball in the center. This infrared picture was taken by NASA Spitzer Space Telescope. ",
                        "date_created": "2008-10-01T16:14:27Z",
                        "keywords": [
                            "Cassiopeia A",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA11213",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08453/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08453/PIA08453~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "NASA Spitzer Space Telescope caught a glimpse of the Cepheus constellation, thirty thousand light-years away; astronomers think theyve found a massive star whose death barely made a peep. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/NASA Herschel Science Center/DSS",
                        "title": "The Almost Invisible Aftermath of a Massive Star Death",
                        "description": "NASA Spitzer Space Telescope caught a glimpse of the Cepheus constellation, thirty thousand light-years away; astronomers think theyve found a massive star whose death barely made a peep. ",
                        "date_created": "2006-05-11T18:06:36Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA08453",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA08533/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA08533/PIA08533~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "Astronomers using NASA Spitzer Space Telescope have spotted a dust factory 30 million light-years away in the spiral galaxy M74. The factory is located at the scene of a massive star explosive death, or supernova. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech/STScI",
                        "title": "Supernova Dust Factory in M74",
                        "description": "Astronomers using NASA Spitzer Space Telescope have spotted a dust factory 30 million light-years away in the spiral galaxy M74. The factory is located at the scene of a massive star explosive death, or supernova. ",
                        "date_created": "2006-06-09T21:15:22Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA08533",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA09263/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA09263/PIA09263~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "The Seven Sisters, also known as the Pleiades star cluster, seem to float on a bed of feathers in a new infrared image from NASA Spitzer Space Telescope. Clouds of dust sweep around the stars, swaddling them in a cushiony veil. ",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "The Seven Sisters Pose for Spitzer",
                        "description": "The Seven Sisters, also known as the Pleiades star cluster, seem to float on a bed of feathers in a new infrared image from NASA Spitzer Space Telescope. Clouds of dust sweep around the stars, swaddling them in a cushiony veil. ",
                        "date_created": "2007-04-16T16:19:07Z",
                        "keywords": [
                            "Pleiades",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA09263",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA18847/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA18847/PIA18847~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This image from NASA Spitzer Space Telescope shows where the action is taking place in galaxy NGC 1291. The outer ring, colored red, is filled with new stars that are igniting and heating up dust that glows with infrared light.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Ring of Stellar Fire",
                        "description": "This image from NASA Spitzer Space Telescope shows where the action is taking place in galaxy NGC 1291. The outer ring, colored red, is filled with new stars that are igniting and heating up dust that glows with infrared light.",
                        "date_created": "2014-10-22T16:00:13Z",
                        "keywords": [
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA18847",
                        "media_type": "image"
                    }
                ]
            },
            {
                "href": "https://images-assets.nasa.gov/image/PIA01902/collection.json",
                "links": [
                    {
                        "rel": "preview",
                        "href": "https://images-assets.nasa.gov/image/PIA01902/PIA01902~thumb.jpg",
                        "render": "image"
                    }
                ],
                "data": [
                    {
                        "description_508": "This artist concept shows the explosion of a massive star, the remains of which are named Cassiopeia A. NASA Spitzer Space Telescope found evidence that the star exploded with some degree of order.",
                        "center": "JPL",
                        "secondary_creator": "NASA/JPL-Caltech",
                        "title": "Order Amidst Chaos of Star Explosion  Artist Concept",
                        "description": "This artist concept shows the explosion of a massive star, the remains of which are named Cassiopeia A. NASA Spitzer Space Telescope found evidence that the star exploded with some degree of order.",
                        "date_created": "2006-10-26T17:57:51Z",
                        "keywords": [
                            "Cassiopeia A",
                            "Spitzer Space Telescope"
                        ],
                        "nasa_id": "PIA01902",
                        "media_type": "image"
                    }
                ]
            }
        ]
    }
}

     */
}
