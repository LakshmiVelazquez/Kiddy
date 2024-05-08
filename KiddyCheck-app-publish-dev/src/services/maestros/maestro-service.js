import { $apiService } from '../../utils/api-services';

export default {
    obtenerPersonasPorTipo: async (id) => {
        try {
            let res = await $apiService.get(`/api/v1/Personas/ObtenerPersonaPorTipo?id=${id}`)
            return res.data;
        } catch (error) {
            console.log(error);
            return null
        }
    },
    agregarPersona: async (persona) => {
        try {
            let res = await $apiService.post(`/api/v1/Personas/AgregarPersona`, persona)
            return res
        } catch (error) {
            console.log(error);
            return null
        }
    },

    editarPersona: async (persona) => {
        try {
            let res = await $apiService.post(`/api/v1/Personas/ActualizarPersona`, persona)
            return res
        } catch (error) {
            console.log(error);
            return null
        }
    },
    eliminarPersona: async (id) => {
        try {
            let res = await $apiService.delete(`/api/v1/Personas/EliminarPersona?id=${id}`)
            return res
        } catch (error) {
            console.log(error);
            return null
        }
    },
}