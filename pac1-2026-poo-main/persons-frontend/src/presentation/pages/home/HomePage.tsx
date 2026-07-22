import { Lock, Users } from "lucide-react";
import { DashboardCard } from "../../components";
import { useStatistics } from "../../hooks";
import { Loader } from "../../components/share/Loader";

export const HomePage = () => {
  const { data : response, isLoading } = useStatistics();

   if(isLoading){
     return <Loader/>
  }

  return (
    <div className="p-4">
      <h1 className="text-3xl text-blue-400 mb-4">Página de Inicio</h1>

      {/* Cards de estadisticas */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-2">
        <DashboardCard
          countValue={response.data.personsCount}
          to="/persons/create"
          title="Personas"
          icon={<Users size={48} />}
        />
        <DashboardCard
          countValue={response.data.usersCount}
          to="/users/create"
          title="Usuarios"
          icon={<Lock size={48} />}
        />
      </div>
    </div>
  );
};
