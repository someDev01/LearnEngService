import { BadgeCheckIcon, BookCheck, PlayCircleIcon, PlusCircleIcon, PlusSquareIcon, TextAlignStart } from 'lucide-react';
import styles from '../last_activity/last_activity.module.css';
import Title from '../title/Title';
import ProfileForm from '../profile_form/ProfileForm';
import ProfileItem from '../profile_item/ProfileItem';

function LastActivity({activities = []}){

    const activitiesConfig = {
        Added: {
            Icon: PlusCircleIcon,
            title: 'Добавлено слово',
            color: '#ff5e00'
        },
        Trained:{
            Icon: BookCheck,
            title: 'Изученное слово',
            color: '#00ff66'
        }
    };

    return(
        <ProfileForm title="Последняя активность">
            {activities.map(act => {
                const config = activitiesConfig[act.type];

                return(
                    <ProfileItem
                        key={act.type}
                        Icon={config.Icon}
                        title={config.title}
                        subtitle={act.word}
                        date={act.date}
                        color={config.color}
                    />
                )
            })}
        </ProfileForm>
    )
}

export default LastActivity;