import { getAccessToken } from "@ikco/mfe-core";
import { HubConnectionBuilder, HubConnectionState, LogLevel, type HubConnection } from "@microsoft/signalr";
import { useEffect, useRef, useState } from "react";

/** کانال‌های به‌روزرسانی زنده (PfLiveChannels در بک‌اند) */
export type LiveChannel = "body-tracking" | "body-preassign" | "trim-tracking";

const HUB_PATH = "/signalr-hubs/production-feasibility";
const RETRY_MS = 30000;

/**
 * عضویت صفحه گزارش در یک کانال SignalR و فراخوانی onChange هنگام تغییر داده‌های گزارش در پایگاه داده
 * (جایگزین Service Broker فرم‌های ویندوزی). در صورت در دسترس نبودن سرور، بی‌صدا هر ۳۰ ثانیه دوباره تلاش می‌کند.
 * خروجی connected برای نمایش وضعیت «به‌روزرسانی زنده» است.
 */
export default function useLiveRefresh(channel: LiveChannel, onChange: () => void, enabled = true) {
  const [connected, setConnected] = useState(false);
  const handler = useRef(onChange);
  handler.current = onChange;

  useEffect(() => {
    if (!enabled) return;
    let disposed = false;
    let retry: number | undefined;
    const connection: HubConnection = new HubConnectionBuilder()
      .withUrl(`${import.meta.env.VITE_API_BASE_URL ?? ""}${HUB_PATH}`, {
        accessTokenFactory: async () => (await getAccessToken?.()) ?? "",
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.None)
      .build();

    connection.on("changed", (changed: string) => {
      if (changed === channel) handler.current();
    });
    connection.onreconnecting(() => setConnected(false));
    connection.onreconnected(async () => {
      await connection.invoke("Subscribe", channel).catch(() => undefined);
      setConnected(true);
      // تغییرات زمان قطع ارتباط از دست نرود
      handler.current();
    });
    connection.onclose(() => {
      setConnected(false);
      if (!disposed) retry = window.setTimeout(start, RETRY_MS);
    });

    async function start() {
      if (disposed || connection.state !== HubConnectionState.Disconnected) return;
      try {
        await connection.start();
        await connection.invoke("Subscribe", channel);
        if (!disposed) setConnected(true);
      } catch {
        setConnected(false);
        if (!disposed) retry = window.setTimeout(start, RETRY_MS);
      }
    }

    void start();
    return () => {
      disposed = true;
      window.clearTimeout(retry);
      void connection.stop();
    };
  }, [channel, enabled]);

  return { connected };
}
