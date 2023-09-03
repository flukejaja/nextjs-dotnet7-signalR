import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { HubConnection } from "@microsoft/signalr";
import { connection } from "../service/SignalR";
import {
  Table,
  TableHeader,
  TableBody,
  TableColumn,
  TableRow,
  TableCell,
} from "@nextui-org/react";
import { useRouter } from "next/router";
import { GetServerSideProps } from "next";
import { getCustomer , putCustomer } from "@/api/customerAPI";
import { useQuery } from "react-query";
import { Customer } from "@/model/cusotmer";
import { ObjectId } from 'bson';


export default function Home({ ip }: { ip: string }) {
  const [signalRConnection, setSignalRConnection] = useState<HubConnection>();
  const [customerInfo, setCustomerInfo] = useState<Customer[]>([]);
  const { data , refetch} = useQuery("customerInfo", getCustomer , {enabled: false,
    staleTime: Infinity});
  const [getCustomerId , setCustomerId] = useState<string>("");
  useEffect(() => {
    if (
      connection &&
      connection.state === signalR.HubConnectionState.Disconnected
    ) {
      connection
        .start()
        .then(function () {
          setSignalRConnection(connection);
        })
        .catch(function (err) {
          return console.error(err.toString());
        });
    }
    refetch();
  }, []);

  useEffect(() => {
    setCustomerInfo(data?.data ?? []);
  }, [data]);

  useEffect(() => {
    let generateCustomer = new ObjectId().toHexString();
    setCustomerId(generateCustomer)
    let body = {
      id: generateCustomer,
      name: ip,
      getStart: new Date().toLocaleTimeString(),
      finishTime: new Date().toLocaleTimeString(),
      time: "",
    };
    if (signalRConnection) {
      signalRConnection.invoke("ReceiveMessage", JSON.stringify(body));
    }
  }, [signalRConnection]);

  useEffect(() => {

    if (signalRConnection) {
      signalRConnection.on("ReceiveMessage", (message) => {
        if(message.data.id !== customerInfo[customerInfo.length - 1]?.id){
          customerInfo.push(message.data);
          setCustomerInfo([...customerInfo]);
          showNotification(true);
        }
      });
      signalRConnection.on("UpdateMessage", (message) => {
        let getIndex = customerInfo.findIndex(x => x.id === message.data.id);
        customerInfo[getIndex] = message.data
        setCustomerInfo([...customerInfo]);
        showNotification(false);
      });
    }
  }, [signalRConnection , getCustomerId]);

  useEffect(() => {
    const exitingFunction = async () => {
      if (signalRConnection) {
        signalRConnection.invoke(
          "UpdateMessage",
          getCustomerId
        );
      }
    };
    
    window.onbeforeunload = exitingFunction;
  }, [getCustomerId , signalRConnection]);

  function showNotification(checked: boolean) {
    if (Notification.permission !== 'granted') {
      Notification.requestPermission();
    } else {
      const options = {
        body: `${getCustomerId} is ${checked ? 'coming soon' : 'exiting'}`,
        dir: 'ltr',
        image: 'image.jpg'
      } as NotificationOptions;
      const notification = new Notification('Notification', options  );
      notification.onclick = function () {
        window.open('https://www.google.com');
      };
    }
  }

  return (
    <main
      className={`flex min-h-screen flex-col items-center justify-between p-24`}
    >
      <Table aria-label="Example static collection table">
        <TableHeader>
          <TableColumn>ID</TableColumn>
          <TableColumn>IPAddress</TableColumn>
          <TableColumn>GetStart</TableColumn>
          <TableColumn>FinishTime</TableColumn>
          <TableColumn>Time</TableColumn>
        </TableHeader>
        <TableBody>
          {customerInfo.map((row, idx) => (
            <TableRow key={row.id ?? idx}>
              <TableCell>{row.id}</TableCell>
              <TableCell>{row.name}</TableCell>
              <TableCell>{row.getStart}</TableCell>
              <TableCell>{row.finishTime}</TableCell>
              <TableCell>{row.time}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </main>
  );
}
export const getServerSideProps: GetServerSideProps = async ({ req }) => {
  const forwarded = req.headers["x-forwarded-for"];
  const ip =
    typeof forwarded === "string"
      ? forwarded.split(/, /)[0]
      : req.socket.remoteAddress;
  return {
    props: { ip },
  };
};
