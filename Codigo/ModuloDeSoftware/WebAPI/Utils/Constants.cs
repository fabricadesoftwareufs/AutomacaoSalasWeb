using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class Constants
    {
        public static readonly string[] ROUTES_HARDWARE = {
            "/api/hardwaredesala/info",
            "/api/hardwaredesala/get-sensors-and-actuators",
            "/api/hardwaredesala/register",
            "/api/HorarioSala/ReservasDeHojePorUuid",
            "/api/hardwaredesala/slave/get-master",
            "/api/monitoramento",
            "/api/infravermelho/CodigosPorUuid"
        };
    }
}
