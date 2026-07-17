import { User2 } from 'lucide-react';
import styles from '../profile/profile.module.css';

function Profile({user}){
    return(
        <div className={styles.profile_block}>
            <div className={styles.profile_circle}>
                <p>{user?.email[0].trim().toUpperCase()}</p>
            </div>
            {user?.name && (
                <div className={styles.user_name}>
                    <p>{user?.name}</p>
                </div>
            )}
            <div className={styles.user_email}>
                <p>{user?.email}</p>
            </div>
        </div>
    )
}

export default Profile; 