import axios from "axios";
import { ResultList } from "@/model/result";
import { Customer } from "@/model/cusotmer";

export const getCustomer = (): Promise<ResultList<Customer>> => {
    return new Promise((resolve, reject) => {
      axios
        .get("http://localhost:5182/customer/getCustomer")
        .then((response) => {
          resolve(response.data);
        })
        .catch((error) => {
          reject(error);
        });
    });
  };

export const putCustomer = (id : string): Promise<ResultList<Customer>> => {
    return new Promise((resolve, reject) => {
      axios
        .put(`http://localhost:5182/customer/putCustomer?id=${id}`)
        .then((response) => {
          resolve(response.data);
        })
        .catch((error) => {
          reject(error);
        });
    });
  };
