import styles from '../app_icon/app_icon.module.css';

function AppIcon({Icon, className, color}){
    return(
        <Icon
            className={styles[className]}
            color={color}
        />
    )
}

export default AppIcon;