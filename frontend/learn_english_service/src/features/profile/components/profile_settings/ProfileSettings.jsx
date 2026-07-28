import { Bell, LogOut } from 'lucide-react';
import styles from '../profile_settings/profile_settings.module.css';
import Title from '../title/Title';
import ProfileForm from '../profile_form/ProfileForm';
import ProfileItem from '../profile_item/ProfileItem';
import { authApi } from '../../../../api/auth';
import { resetStep, resetUser, setError } from '../../../../redux/slices/authSlice';
import { toast } from 'react-toastify';
import { closeMenu } from '../../../../redux/slices/menuSlice';
import { useDispatch } from 'react-redux';

function ProfileSettings(){

    const dispatch = useDispatch();
    
    const onOpenNotification = () => {
        console.log('Уведомления открыты');
        
    };

    const onLogout = async() => {
        try{
            const response = await authApi.logout();
            if(response.success){
                dispatch(resetStep());
                dispatch(resetUser());
                toast.success('Вы вышли из аккаунта');
            }
            else{
                dispatch(setError(response.error));
                toast.error(response.error);
            }
        }
        catch(e){
            toast.error(e.error.message);
            e.response?.data?.message || e.message;
        }
        finally{
            dispatch(closeMenu());
            navigation('/');
        }
    }

    const items = [
        {
            id: 1,
            Icon: Bell,
            title: 'Уведомления',
            color: '#e9e9e9',
            onClick : onOpenNotification
        },
        {
            id:2,
            Icon: LogOut,
            title: 'Выйти из аккаунта',
            color: '#ea0000',
            type: 'exit',
            onClick: onLogout
        }
    ];

    return(
        <ProfileForm title="Настройки">
            {items.map(item => (
                <ProfileItem
                    key={item.id}
                    Icon={item.Icon}
                    title={item.title}
                    color={item.color}
                    type={item.type}
                    onClick={item.onClick}
                />
            ))}
        </ProfileForm>
    )
}

export default ProfileSettings;