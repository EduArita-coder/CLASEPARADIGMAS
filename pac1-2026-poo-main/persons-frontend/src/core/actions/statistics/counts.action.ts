import type { AxiosError } from "axios";
import type {
  ApiErrorResponse,
  StatisticsResponse,
} from "../../../infrastructure/interfaces";
import type { ApiResponse } from "../../../infrastructure/interfaces/api.response";
import { personsApi } from "../../api";

export const countsAction = async () => {
  try {
    const { data: body } =
      await personsApi.get<ApiResponse<StatisticsResponse>>("statistics");

    return body;

  } 
  catch (error) {
    const apiError = error as AxiosError<ApiErrorResponse>;
    console.log(apiError);

    if(apiError.response){
       throw new Error(apiError.response.data.message, apiError.cause)
    } else if(apiError.request){
      throw new Error("Error de Conexión",apiError.cause);
    }else{
       throw new Error("Error desconocido",apiError.cause);
    }

  }
};
