
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Model.ViewModel
{
    public class ModeloEquipamentoViewModel
    {
        public ModeloEquipamentoViewModel()
        {
            Codigos = new List<CodigoInfravermelhoViewModel>();
        }

        public ModeloEquipamentoModel ModeloEquipamento { get; set; }

        public List<ModeloEquipamentoModel> ModeloEquipamentoModels { get; set; } = new List<ModeloEquipamentoModel>();

        public List<CodigoInfravermelhoViewModel>? Codigos { get; set; } = new List<CodigoInfravermelhoViewModel>();

        public List<MarcaEquipamentoViewModel> Marcas { get; set; } = new List<MarcaEquipamentoViewModel>();
    }
}
