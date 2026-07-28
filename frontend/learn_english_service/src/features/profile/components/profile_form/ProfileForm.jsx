import styles from '../profile_form/profile_form.module.css';
import Title from '../title/Title';

function ProfileForm({title, children}){

    return(
        <section className={styles.form}>
            <Title title={title}/>

            <div className={styles.items}>
                {children}
            </div>
        </section>
    )
}

export default ProfileForm;