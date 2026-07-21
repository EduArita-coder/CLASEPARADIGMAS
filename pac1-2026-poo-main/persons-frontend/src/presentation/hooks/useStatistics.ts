import { useQuery } from "@tanstack/react-query";
import { countsAction } from "../../core/actions";

export const useStatistics = () => {
    const {data, isLoading,error} = useQuery({
        queryKey : ["statistics/counts"],
        queryFn : () => countsAction(),
        staleTime : 0,
        refetchOnWindowFocus : false,
        
    });
    return {
        //Propiedades 
        data,
        isLoading,
        error,
        //Metodos 
        
    }
}