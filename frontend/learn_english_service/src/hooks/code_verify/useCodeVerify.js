import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { authApi } from "../../api/auth";
import { resetResendTime } from "../../redux/slices/authSlice";
import { convertToTimeSeconds } from "../../utils/convert_time/convertToTimeSeconds";

export function useCodeVerify({ tempUser }) {
    const dispatch = useDispatch();
    const resendTime = useSelector(state => state.auth.resendTime);
    const [leftResendTime, setLeftResendTime] = useState(0);

    useEffect(() => {
        if (!resendTime) return;

        const interval = setInterval(() => {
            const left = Math.floor((resendTime - Date.now()) / 1000);

            if (left > 0) setLeftResendTime(left);
            else {
                setLeftResendTime(0);
                dispatch(resetResendTime());
            }
        }, 1000);

        return () => clearInterval(interval);
    }, [resendTime]);

    const verifyCode = async (code) => {
        if (!tempUser) {
            return { success: false, error: "Нет пользователя" };
        }

        try {
            const response = await authApi.verifyCode(tempUser.email, code);

            if (response.success) {
                return { success: true, data: response.data };
            }

            return {
                success: false,
                error: response.error 
            };

        } catch (error) {
            return {
                success: false,
                error: error.response?.data?.message || error.message
            };
        }
    };

    const sendCodeAgain = async () => {
        if (!tempUser || leftResendTime > 0) {
            return { success: false, error: "Повторная отправка кода пока недоступна" };
        }

        try {
            const response = await authApi.sendCode(tempUser.email);

            if (response.success) {
                const seconds = convertToTimeSeconds(response.data.time);

                return {
                    success: true,
                    data: {
                        resendTime: Date.now() + seconds * 1000
                    }
                };
            }

            return {
                success: false,
                error: response.message
            };

        } catch (error) {
            return {
                success: false,
                error: error.response?.data?.message || error.message
            };
        }
    };

    return {
        leftResendTime,
        verifyCode,
        sendCodeAgain,
        isResendDisabled: leftResendTime > 0
    };
}