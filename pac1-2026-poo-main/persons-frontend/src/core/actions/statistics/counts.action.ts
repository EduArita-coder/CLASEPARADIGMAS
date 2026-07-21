import { personsApi } from "../../api";

export const countsAction = async () => {
  try 
  {
     const {data : body} = await personsApi.get('statistics');

     return body;
  } 
  catch (error) 
  {
     console.log(error);
  }
};
