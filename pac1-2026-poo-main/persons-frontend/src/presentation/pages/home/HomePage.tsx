import { Lock, Users } from "lucide-react";
import { DashboardCard } from "../../components";
import { useEffect } from "react";
import { countsAction } from "../../../core/actions/statistics/counts.action";

export const HomePage = () => {
  useEffect(()=>{
    const body = countsAction();
    console.log(body);
    
  }, []);
  return (
    <div className="p-4">
      <h1 className="text-3xl text-blue-400 mb-4">Página de Inicio</h1>

      {/* Cards de estadusticas */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
        <DashboardCard
          countValue={100000}
          to="/persons/create"
          title="Personas"
          icon={<Users size={48} />}
        />
        <DashboardCard
          countValue={30}
          to="/users/create"
          title="Usuarios"
          icon={<Lock size={48} />}
        />
      </div>
    </div>
  );
};
