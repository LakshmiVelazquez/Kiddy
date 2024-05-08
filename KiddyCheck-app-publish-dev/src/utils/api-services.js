import { useCookie } from '../@core/composable/useCookie'
import axios from 'axios'

export const $apiService = {
    //urlApi: `${import.meta.env.VITE_API_URL}/api/v1/`,
    urlApi: "https://localhost:7163",


    config: {
        headers: {
            'authorization': `Bearer ${useCookie('accessToken').value + ''}`
        }
    },

    post(url, payload) {
        console.log(this.urlApi + url, this.config);
        return axios.post(this.urlApi + url, payload, this.config)
    },

    put(url, payload) {
        console.log(this.urlApi + url);
        return axios.put(this.urlApi + url, payload, this.config)
    },

    get(url) {
        console.log(this.urlApi + url, this.config);
        // console.log(this.config);
        return axios.get(this.urlApi + url, this.config)
    },

    delete(url) {
        console.log(this.urlApi + url);
        return axios.delete(this.urlApi + url, this.config)
    },

}