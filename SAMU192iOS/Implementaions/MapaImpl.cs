using System.Threading.Tasks;
using CoreLocation;
using MapKit;
using UIKit;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using System.Collections.Generic;

namespace SAMU192iOS.Implementaions
{
    internal class MapaImpl : IMapa
    {
        private MKMapView oMap;
        IMKAnnotation[] annotations = new MKPointAnnotation[20];
        MKCoordinateSpan Span = new MKCoordinateSpan(0.015, 0.015);

        public void Carrega(object args, CoordenadaDTO coordenadaDefault, bool overrideDelegate)
        {
            this.oMap = (MKMapView)args;
            this.oMap.Delegate = (overrideDelegate ? new MyMapDelegate("Imagens/ambulancia_verde.png") : null);
            PosicionaMapa(coordenadaDefault);
            RemoveAllPins();
        }

        private void RemoveAllPins()
        {
            int toRemoveCount = oMap.Annotations.Length;
            List<IMKAnnotation> toRemove = new List<IMKAnnotation>(toRemoveCount);
            foreach (IMKAnnotation annotation in oMap.Annotations)
            {
                if (annotation != oMap.UserLocation)
                    toRemove.Add(annotation);
                oMap.RemoveAnnotations(toRemove.ToArray());
            }
        }

        public CoordenadaDTO PosicionaMapa(CoordenadaDTO coordenada, float? zoom = null)
        {
            if (oMap != null && coordenada != null)
            {
                CLLocationCoordinate2D location = new CLLocationCoordinate2D(coordenada.Latitude, coordenada.Longitude);
                MKCoordinateRegion region = new MKCoordinateRegion(location, Span);
                oMap.SetCenterCoordinate(location, true);
                oMap.SetRegion(region, true);
            }
            return coordenada;
        }

        public async Task<EnderecoDTO> ReverterCoordenada(CoordenadaDTO coordenada, object args = null)
        {
            EnderecoDTO result = null;
            if (coordenada != null)
            {
                var geoCoder = new CLGeocoder();
                CLLocation location = new CLLocation(coordenada.Latitude, coordenada.Longitude);
                var placemarks = await geoCoder.ReverseGeocodeLocationAsync(location);
                if (placemarks != null && placemarks.Length > 0)
                {
                    result = CompilarEndereco(coordenada, placemarks[0]);
                }
            }
            return result;
        }

        public async Task<EnderecoDTO> ReverterEndereco(string enderecoLegivel, object args = null)
        {
            EnderecoDTO result = null;
            if (!string.IsNullOrEmpty(enderecoLegivel))
            {
                var geoCoder = new CLGeocoder();
                var placemarks = await geoCoder.GeocodeAddressAsync(enderecoLegivel);
                if (placemarks != null && placemarks.Length > 0)
                {
                    var rad = placemarks[0].Region.Radius;

                    var coordenada = new CoordenadaDTO(placemarks[0].Location.Coordinate.Latitude, placemarks[0].Location.Coordinate.Longitude);
                    result = CompilarEndereco(coordenada, placemarks[0]);
                }
            }
            return result;
        }

        private EnderecoDTO CompilarEndereco(CoordenadaDTO coordenada, CLPlacemark placemark)
        {
            EnderecoDTO result = null;
            string subLocality = placemark.SubLocality;
            subLocality = (subLocality != null && subLocality.ToLower() == "centro" ? "Centro" : subLocality);
            string thoroughfare = string.Empty;
            if (!string.IsNullOrEmpty(placemark.Thoroughfare))
                thoroughfare = (placemark.Thoroughfare.Length >= 7 && placemark.Thoroughfare.Trim().ToLower().Substring(0, 7) == "avenida"
                                    ? placemark.Thoroughfare.Replace(placemark.Thoroughfare.Trim().Substring(0, 7), "Av.")
                                    : placemark.Thoroughfare);
            result = new EnderecoDTO(thoroughfare, placemark.SubThoroughfare, "", subLocality, placemark.Locality, placemark.AdministrativeArea, coordenada);
            return result;
        }

        public object ConfigurarMapa(object view)
        {
            return null;
        }

        public void LimparMapa()
        {
            this.oMap.RemoveAnnotations();
        }

        public CoordenadaDTO PosicionaImagemNoMapa(CoordenadaDTO coordenada, string imagemPath, object tela, int index, string nomeEquipe)
        {
            if (oMap != null && coordenada != null)
            {
                CLLocationCoordinate2D location = new CLLocationCoordinate2D(coordenada.Latitude, coordenada.Longitude);

                MKPointAnnotation myHomePin;
                if (annotations[index] != null)
                {
                    //oMap.RemoveAnnotation(annotations[index]);
                    myHomePin = (MKPointAnnotation)annotations[index];
                }
                else
                {
                    myHomePin = new MKPointAnnotation();
                }
                myHomePin.Coordinate = location;
                myHomePin.Title = string.Empty;//nomeEquipe;
                myHomePin.Subtitle = string.Empty;
                oMap.AddAnnotation(myHomePin);

                string reuseId = "PinAnnotation";
                MKAnnotationView anView = oMap.DequeueReusableAnnotation(reuseId);
                if (anView == null)
                {
                    anView = new MKAnnotationView(myHomePin, reuseId);
                    anView.Annotation = myHomePin;
                    anView.CanShowCallout = true;
                    anView.Enabled = true;
                    anView.Image = new UIImage(imagemPath);
                    anView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile(imagemPath));
                    anView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
                }
                else
                {
                    anView.Annotation = myHomePin;
                }

                annotations[index] = myHomePin;
            }
            return coordenada;
        }

        public void AjustaCentralizacaoMapa(CoordenadaDTO coordenada, CoordenadaDTO coordenadaAmbulancia, double fator, bool alterouDestino)
        {
            CLLocationCoordinate2D location;
            if (alterouDestino)
            {
                location = new CLLocationCoordinate2D(coordenada.Latitude, coordenada.Longitude);
                Span = new MKCoordinateSpan(fator, fator);
                MKCoordinateRegion regionAux = new MKCoordinateRegion(location, Span);
                oMap.SetRegion(regionAux, true);
                return;
            }
            else
            {
                if (oMap.Region.Span.LatitudeDelta < (fator - 0.001))
                {
                    location = new CLLocationCoordinate2D(coordenadaAmbulancia.Latitude, coordenadaAmbulancia.Longitude);
                }
                else
                {
                    location = new CLLocationCoordinate2D(coordenada.Latitude, coordenada.Longitude);
                    Span = new MKCoordinateSpan(fator, fator);
                }
            }
            MKCoordinateRegion region = new MKCoordinateRegion(location, (oMap.Region.Span.LatitudeDelta > 25) ? Span : oMap.Region.Span);
            oMap.SetRegion(region, true);
        }

        internal class MyMapDelegate : MKMapViewDelegate
        {
            internal MyMapDelegate(string _imgPath)
            {
                imgPath = _imgPath;
            }

            protected string imgPath;
            protected string annotationIdentifier = "PinAnnotation";

            public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
            {
                MKAnnotationView anView;

                if (annotation is MKUserLocation)
                    return null;

                anView = mapView.DequeueReusableAnnotation(annotationIdentifier);

                if (anView == null)
                    anView = new MKAnnotationView(annotation, annotationIdentifier);

                anView.Image = GetImage(imgPath);
                anView.CanShowCallout = true;

                return anView;
            }

            private UIImage GetImage(string imageName)
            {
                var image = UIImage.FromFile(imageName);

                return image;
            }
        }
    }
}