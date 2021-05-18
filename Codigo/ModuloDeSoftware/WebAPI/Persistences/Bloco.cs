﻿using System.Collections.Generic;

namespace Persistence
{
    public partial class Bloco
    {
        public Bloco()
        {
            Hardwaredebloco = new HashSet<Hardwaredebloco>();
            Sala = new HashSet<Sala>();
        }

        public int Id { get; set; }
        public int Organizacao { get; set; }
        public string Titulo { get; set; }

        public Organizacao OrganizacaoNavigation { get; set; }
        public ICollection<Hardwaredebloco> Hardwaredebloco { get; set; }
        public ICollection<Sala> Sala { get; set; }
    }
}