using System.Linq;

using SAMU192Core.BLL;

namespace SAMU192Core.DTO
{
    public class AreaDTO
    {

        private Area area;

        internal Area Area { get { return area; } }

        public AreaDTO()
        {
            area = new Area();
        }

        internal AreaDTO(Area area)
        {
            this.area = area;
        }

        public int Cod { get { return area.cod; } set { area.cod = value; } }
        public int CodServidor { get { return area.codServidor; } set { area.codServidor = value; } }
        public string Fonte { get { return area.fonte; } set { area.fonte = value; } }
        public PoligonoDTO Limites { get { return new PoligonoDTO(area.Limites); } }
        public PoligonoDTO getPoligono() 
        {
            return new PoligonoDTO(area.Poligono);
        }
        public void setPoligono(PoligonoDTO poligono)
        {
            area.Poligono = new Poligono(poligono.getPontos().Select(x=>new Coordenada(x.Latitude, x.Longitude)).ToList());
        }

        public override string ToString()
        {
            return area.ToString();
        }

    }
}
