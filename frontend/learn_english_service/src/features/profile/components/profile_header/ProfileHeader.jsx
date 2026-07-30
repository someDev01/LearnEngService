import { Camera } from 'lucide-react';
import styles from '../profile_header/profile_header.module.css';
import { userApi } from '../../api/user';
import { setUser } from '../../../../redux/slices/authSlice';
import { useDispatch } from 'react-redux';

function ProfileHeader({user}){

    const dispatch = useDispatch();

    const handleUpload = async(e) => {
        const file = e.target.files[0];

        if(!file) return;

        const formData = new FormData();
        formData.append("file", file);

        const response = await userApi.uploadAvatar(formData);

        if(response.success){
            dispatch(setUser({
                ...user,
                avatarUrl: `${response.data?.avatarUrl}?t=${Date.now()}`
            }))
        }
        else{
            console.log(response.error);
            
        }
    };

    return(  
        <header className={styles.profile_header}>
            <h3>Профиль</h3>
            <section className={styles.user_data_section}>
                <div className={styles.avatar}>
                    <input
                        id="avatar"
                        type="file"
                        accept="image/*"
                        hidden
                        onChange={handleUpload}
                    />
                    <label htmlFor="avatar" className={styles.avatarLabel}>
                        {user?.avatarUrl ? <img src={user?.avatarUrl} alt="Аватар" /> : <p>Загрузить изображение</p>}
                    </label>
                    <div className={styles.icon_photo}>
                        <Camera size={18} color="white" />
                    </div>
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