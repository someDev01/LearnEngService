import { Camera } from 'lucide-react';
import styles from '../profile_header/profile_header.module.css';

function ProfileHeader({user}){
    return(  
        <header className={styles.profile_header}>
            <h3>Профиль</h3>
            <section className={styles.user_data_section}>
                <div className={styles.avatar}>
                    <div className={styles.icon_photo}>
                        <Camera size={18} color='white'/>
                    </div>
                    <img src='/avatar.png' alt=''/>
                </div>
                <div className={styles.user_data}>
                    <div className={styles.name_and_email}>
                        <span className={styles.name}>{user?.name}</span>
                        <span className={styles.email}>{user?.email}</span>
                    </div>
                </div>
            </section>
        </header>
    )
}

export default ProfileHeader;