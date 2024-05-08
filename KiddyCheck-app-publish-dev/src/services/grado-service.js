//import { $apiService } from "../utils/api-services";

import axios from "axios";

const controller = 'api/Grado/'
const APIURL = import.meta.env.VITE_API_URL;
export default {
    ObtenerGrados: async () => {
        try {
            let res = await axios.get(`${APIURL}/${controller}ObtenerGrados`);
            return res.data;
        } catch (error) {
            console.error('Error al agregar grado' + error);
            return error;
        }
    },
    ObtenerGradoId: async (id) => {
        try {
            let res = await axios.get(`${APIURL}/${controller}ObtenerGradoId?Id=${id}`);
            return res.data;
        } catch (error) {
            console.error('Error al agregar grado' + error);
            return error;
        }
    },
    AgregarGrados: async (persona) => {
        try {
            let res = await axios.post(`${APIURL}/${controller}AgregarGrado`, persona)
            return res
        } catch (error) {
            console.log(error);
            return error
        }
    },

    EditarGrados: async (persona) => {
        try {
            let res = await axios.put(`${APIURL}/${controller}ModificarGrado`, persona)
            return res
        } catch (error) {
            console.log(error);
            return error
        }
    },
    EliminarGrados: async (id) => {
        try {
            let res = await axios.delete(`${APIURL}/${controller}EliminarGrado?id=${id}`)
            return res
        } catch (error) {
            console.log(error);
            return error
        }
    },
}