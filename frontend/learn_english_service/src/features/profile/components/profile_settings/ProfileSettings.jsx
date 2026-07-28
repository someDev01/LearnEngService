import { Bell, LogOut } from 'lucide-react';
import styles from '../profile_settings/profile_settings.module.css';
import Title from '../title/Title';
import ProfileForm from '../profile_form/ProfileForm';
import ProfileItem from '../profile_item/ProfileItem';

function ProfileSettings(){

    const onOpenNotification = () => {
        console.log('Уведомления открыты');
        
    };

    const onLogout = () => {console.log('Выход из аккаунта произошел');
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