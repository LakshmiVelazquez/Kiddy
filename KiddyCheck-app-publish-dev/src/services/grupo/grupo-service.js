//import { $apiService } from "../utils/api-services";

import axios from "axios";

const controller = 'api/Grupo/'
const APIURL = import.meta.env.VITE_API_URL;

export default {
    ObtenerGrupo: async () => {
        try {
            let res = await axios.get(`${APIURL}/${controller}ObtenerGrupos`);
            return res.data; 
        } catch (error) {
            console.error('Error al agregar grado'+ error);
            return null;
        }
    },
    // ObtenerGrupoId: async (id) => {
    //     try {
    //         let res = await axios.get(`https://kiddycheck-api.azurewebsites.net/${controller}ObtenerGradoId?Id=${id}`);
    //         return res.data; 
    //     } catch (error) {
    //         console.error('Error al agregar grado'+ error);
    //         return null;
    //     }
    // },
    AgregarGrupo: async (persona) => {
        try {
            let res = await axios.post(`${APIURL}/${controller}AgregarGrupo`, persona)
            return res
        } catch (error) {
            console.log(error);
            return null
        }
    },

    EditarGrupo: async (persona) => {
        try {
            let res = await axios.put(`${APIURL}/${controller}ModificarGrupo`, persona)
            return res
        } catch (error) {
            console.log(error);
            return null
        }
    },
    EliminarGrupo: async (id) => {
        try {
            let res = await  axios.delete(`${APIURL}/${controller}EliminarGrupo?id=${id}`)
            return res
        } catch (error) {
            console.log(error);
            return null
        }
    },
}